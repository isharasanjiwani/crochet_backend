using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CrochetWorldApi.Data;
using CrochetWorldApi.Dtos.Auth.Requests;
using CrochetWorldApi.Dtos.Auth.Responses;
using CrochetWorldApi.Dtos.Common.Response;
using CrochetWorldApi.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CrochetWorldApi.Services.Auth;

public class AuthService(AppDbContext dbContext, IConfiguration configuration, ILogger<AuthService> logger) : IAuthService
{
    public async Task<Result<UserResponseDto?>> CreateUserAsync(CreateUserRequestDto request)
    {
        try
        {
            if(await dbContext.Users.AnyAsync(u => u.Email == request.Email && u.UserType == request.UserType))
            {
                return Result<UserResponseDto?>.Failure("User already exists", ErrorType.Conflict);
            }

            var user = new UserEntity
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                ShopName = request.ShopName,
                UserType = request.UserType,
                Email = request.Email,
                Location = request.Location,
                MobileNumber = request.MobileNumber,
                ShopTypes = request.ShopTypes,
                PaymentMethods = request.PaymentMethods,
                Password = string.Empty,

                BankDetails = new BankDetails
                {
                    AccountNumber = request.AccountNumber,
                    BankName = request.BankName,
                    AccountHolderName = request.AccountHolderName
                }
            };

            var hashedPassword = new PasswordHasher<UserEntity>()
                .HashPassword(user, request.Password);

            user.Password = hashedPassword;

            dbContext.Users.Add(user);

            var refreshToken = GenerateRefreshToken();
            var refreshExpiresAt = DateTime.UtcNow.AddDays(1);
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                Created = DateTime.UtcNow,
                Expires = refreshExpiresAt
            };
            dbContext.RefreshTokens.Add(refreshTokenEntity);

            await dbContext.SaveChangesAsync();

            logger.LogInformation(
                "Failed to register user with type {UserType}",
                request.UserType
            );

            return Result<UserResponseDto?>.Success(
                new UserResponseDto
                {
                    UserId = user.Id,
                    UserType = user.UserType,
                    AccessToken = CreateToken(user),
                    RefreshToken = refreshToken
                },
                SuccessType.Created
            );
        } catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to register user with type {UserType}",
                request.UserType
            );
            throw;
        }
    }

    public async Task<Result<UserResponseDto?>> LoginUserAsync(LoginUserRequestDto request)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email && u.UserType == request.UserType);
        if(user is null)
        {
            return Result<UserResponseDto?>.Failure("Invalid credentials", ErrorType.Unauthorized);
        }

        if(new PasswordHasher<UserEntity>().VerifyHashedPassword(user, user.Password, request.Password) 
            == PasswordVerificationResult.Failed)
        {
            return Result<UserResponseDto?>.Failure("Invalid credentials", ErrorType.Unauthorized);
        }

        var refreshToken = GenerateRefreshToken();
        var refreshExpiresAt = DateTime.UtcNow.AddDays(1);
        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            Created = DateTime.UtcNow,
            Expires = refreshExpiresAt,
            SessionExpiresAt = DateTime.UtcNow.AddDays(30)
        };
        dbContext.RefreshTokens.Add(refreshTokenEntity);

        await dbContext.SaveChangesAsync();

        return Result<UserResponseDto?>.Success(
            new UserResponseDto
            {
                UserId = user.Id,
                UserType = user.UserType,
                AccessToken = CreateToken(user),
                RefreshToken = refreshToken
            },
            SuccessType.Ok
        );
    }

    public async Task<Result<UserResponseDto?>> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        var tokenBytes = Encoding.UTF8.GetBytes(request.RefreshToken);
        var hashRefreshToken = SHA256.HashData(tokenBytes);
        var hashRefreshTokenString = Convert.ToBase64String(hashRefreshToken);

        var existing = await dbContext.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == hashRefreshTokenString);
        
        if (existing is null)
        {
            return Result<UserResponseDto?>.Failure("Unauthorizes User", ErrorType.Unauthorized);
        }

        if (existing.SessionExpiresAt <= DateTime.UtcNow)
        {
            existing.Revoked = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();

            return Result<UserResponseDto?>.Failure("Unauthorizes User", ErrorType.Unauthorized);
        }

        if (!existing.IsActive)
        {
            if (existing.Revoked is not null)
            {
                await RevokeAllActiveTokensAsync(dbContext, existing.UserId);
            }
            return Result<UserResponseDto?>.Failure("Unauthorizes User", ErrorType.Unauthorized);
        }

        var user = await dbContext.Users.FindAsync(existing.UserId);

        if (user is null)
        {
            return Result<UserResponseDto?>.Failure("Unauthorizes User", ErrorType.Unauthorized);
        }

        var newRefreshToken = GenerateRefreshToken();
        var refreshExpiresAt = DateTime.UtcNow.AddDays(1);

        var newTokenBytes = Encoding.UTF8.GetBytes(newRefreshToken);
        var hashNewRefreshToken = SHA256.HashData(newTokenBytes);
        var hashNewRefreshTokenString = Convert.ToBase64String(hashNewRefreshToken);

        existing.Revoked = DateTime.UtcNow;
        existing.ReplacedByToken = hashNewRefreshTokenString;

        dbContext.RefreshTokens.Add(new RefreshToken
        {
            Token = hashNewRefreshTokenString,
            UserId = user.Id,
            Created = DateTime.UtcNow,
            Expires = refreshExpiresAt,
            SessionExpiresAt = existing.SessionExpiresAt
        });

        await dbContext.SaveChangesAsync();

        return Result<UserResponseDto?>.Success(
            new UserResponseDto
            {
                UserId = user.Id,
                UserType = user.UserType,
                AccessToken = CreateToken(user),
                RefreshToken = newRefreshToken
            },
            SuccessType.Ok
        );
    }

    private static async Task RevokeAllActiveTokensAsync(AppDbContext dbContext, Guid userId)
    {
        var activeTokens = await dbContext.RefreshTokens
            .Where(t => t.UserId == userId && t.Revoked == null)
            .ToListAsync();

        foreach (var token in activeTokens)
        {
            token.Revoked = DateTime.UtcNow;
        }

        await dbContext.SaveChangesAsync();
    }

    //TODO: call this from logout api
    private static async Task<IResult> RevokeAsync(string refreshToken, AppDbContext db)
    {
        var token = await db.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == refreshToken);

        if (token is null || !token.IsActive)
        {
            return Results.NotFound("Token not found or already inactive.");
        }

        token.Revoked = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Results.Ok("Refresh token revoked.");
    }

    private string GenerateRefreshToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String(randomBytes);
    }

    private string CreateToken(UserEntity user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.UserType.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Key")!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: configuration.GetValue<string>("Jwt:Issuer"),
            audience: configuration.GetValue<string>("Jwt:Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(5),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}

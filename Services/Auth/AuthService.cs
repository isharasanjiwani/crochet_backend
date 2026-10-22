using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CrochetWorldApi.Data;
using CrochetWorldApi.Dtos.Auth.Requests;
using CrochetWorldApi.Dtos.Auth.Responses;
using CrochetWorldApi.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CrochetWorldApi.Services.Auth;

public class AuthService(AppDbContext context, IConfiguration configuration, ILogger<AuthService> logger) : IAuthService
{
    public async Task<UserResponseDto?> CreateUserAsync(CreateUserRequestDto request)
    {
        try
        {
            if(await context.Users.AnyAsync(u => u.Email == request.Email && u.Role == request.Role))
            {
                return null;
            }

            var user = new UserEntity
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                ShopName = request.ShopName,
                Role = request.Role,
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

            context.Users.Add(user);
            await context.SaveChangesAsync();

            logger.LogInformation(
                "Failed to register user with type {UserType}",
                request.Role
            );

            return await CreateTokenResponse(user);
        } catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to register user with type {UserType}",
                request.Role
            );
            throw;
        }
    }

    public async Task<UserResponseDto?> LoginUserAsync(LoginUserRequestDto request)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email && u.Role == request.Role);
        if(user is null)
        {
            return null;
        }

        if(new PasswordHasher<UserEntity>().VerifyHashedPassword(user, user.Password, request.Password) 
            == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return await CreateTokenResponse(user);
    }

    public async Task<UserResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
        if(user is null) 
            return null;

        return await CreateTokenResponse(user);
    }

    private async Task<UserResponseDto> CreateTokenResponse(UserEntity user)
    {
        return new UserResponseDto
        {
            UserId = user.Id,
            Role = user.Role,
            AccessToken = CreateToken(user),
            RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
        };
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private async Task<UserEntity?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
    {
        var user = await context.Users.FindAsync(userId);
        if(user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return null;
        }

        return user;
    }

    private async Task<string> GenerateAndSaveRefreshTokenAsync(UserEntity user)
    {
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await context.SaveChangesAsync();
        return refreshToken;
    }

    private string CreateToken(UserEntity user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Key")!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: configuration.GetValue<string>("Jwt:Issuer"),
            audience: configuration.GetValue<string>("Jwt:Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}

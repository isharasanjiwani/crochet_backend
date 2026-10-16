using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CrochetWorldApi.Data;
using CrochetWorldApi.Dtos.Auth.Requests;
using CrochetWorldApi.Dtos.Auth.Responses;
using CrochetWorldApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CrochetWorldApi.Services.Auth;

public class AuthService(AppDbContext context, IConfiguration configuration) : IAuthService
{
    public async Task<UserResponseDto?> CreateUserAsync(CreateUserRequestDto request)
    {
        if(await context.Users.AnyAsync(u => u.Email == request.Email && u.Type == request.Type))
        {
            return null;
        }

        var user = new UserModel
        {
            Id = Guid.NewGuid(),
            Type = request.Type,
            Email = request.Email,
            Location = request.Location,
            MobileNumber = request.MobileNumber,
            ShopTypes = request.ShopTypes,
            PaymentMethods = request.PaymentMethods,
            Password = request.Password,

            BankDetails = new BankDetails
            {
                AccountNumber = request.AccountNumber,
                BankName = request.BankName,
                AccountHolderName = request.AccountHolderName
            }
        };

        var hashedPassword = new PasswordHasher<UserModel>()
            .HashPassword(user, request.Password);

        user.Password = hashedPassword;

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return await CreateTokenResponse(user);
    }

    public async Task<UserResponseDto?> LoginUserAsync(LoginUserRequestDto request)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email && u.Type == request.Type);
        if(user is null)
        {
            return null;
        }

        if(new PasswordHasher<UserModel>().VerifyHashedPassword(user, user.Password, request.Password) 
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

    private async Task<UserResponseDto> CreateTokenResponse(UserModel user)
    {
        return new UserResponseDto
        {
            UserId = user.Id,
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

    private async Task<UserModel?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
    {
        var user = await context.Users.FindAsync(userId);
        if(user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return null;
        }

        return user;
    }

    private async Task<string> GenerateAndSaveRefreshTokenAsync(UserModel user)
    {
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await context.SaveChangesAsync();
        return refreshToken;
    }

    private string CreateToken(UserModel user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Type)
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

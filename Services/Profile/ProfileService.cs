using System;
using CrochetWorldApi.Data;
using CrochetWorldApi.Dtos.Common.Response;
using CrochetWorldApi.Dtos.Profile.Response;
using CrochetWorldApi.Enums;
using Microsoft.EntityFrameworkCore;

namespace CrochetWorldApi.Services.Profile;

public class ProfileService(AppDbContext context, ILogger<ProfileService> logger) : IProfileService
{
    public async Task<Result<BaseProfileResDto?>> GetProfileData(Guid id)
    {
        var userData = await context.Users
        .Where(u => u.Id == id)
        .Select(u => u.UserType == UserType.Seller ? new SellerProfileResDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            Location = u.Location,
            MobileNumber = u.MobileNumber,
            ProfileImage = u.ProfileImage,
            ShopName = u.ShopName,
            ShopTypes = u.ShopTypes,
            PaymentMethods = u.PaymentMethods,
            BankDetails = u.BankDetails

        } : new BaseProfileResDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            Location = u.Location,
            MobileNumber = u.MobileNumber,
            ProfileImage = u.ProfileImage,
        })
        .FirstOrDefaultAsync();

        if(userData is null)
        {
            logger.LogWarning(
                "Get Profile Data. UserId {@userId}. Response: Failed. User not Found",
                id
            );
            return Result<BaseProfileResDto?>.Failure("User not Found", ErrorType.NotFound);
        }

        logger.LogWarning(
            "Get Profile Data. UserId {@userId}. Response: Success.",
            id
        );
        return Result<BaseProfileResDto?>.Success(userData);
    }
}

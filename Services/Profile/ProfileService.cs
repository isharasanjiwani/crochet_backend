using System;
using CrochetWorldApi.Data;
using CrochetWorldApi.Dtos.Common.Response;
using CrochetWorldApi.Dtos.Profile.Requests;
using CrochetWorldApi.Dtos.Profile.Response;
using CrochetWorldApi.Enums;
using Microsoft.EntityFrameworkCore;

namespace CrochetWorldApi.Services.Profile;

public class ProfileService(AppDbContext context, ILogger<ProfileService> logger) : IProfileService
{
    public async Task<Result<BaseProfileResDto?>> GetProfileData(Guid id)
    {
        var userData = await context.Users
        .AsNoTracking()
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

    public async Task<Result<string?>> EditProfileData(EditProfileReqDto request, Guid id)
    {
        var profileData = await context.Users.FindAsync(id);

        if(profileData is null)
        {
            logger.LogWarning(
                "Updated Profile data. User Id {@userId}. Response: Failed. User not Found",
                id
            );
            return Result<string?>.Failure("User not Found", ErrorType.NotFound);
        }

        if(request.Name is not null) profileData.Name = request.Name;
        if(request.ShopName is not null) profileData.ShopName = request.ShopName;
        if(request.Location is not null) profileData.Location = request.Location;
        if(request.MobileNumber is not null) profileData.MobileNumber = request.MobileNumber;
        if(request.ShopTypes is not null) profileData.ShopTypes = request.ShopTypes;
        if(request.PaymentMethods is not null) profileData.PaymentMethods = request.PaymentMethods;
        if(request.AccountNumber is not null || request.BankName is not null || request.AccountHolderName is not null)
        {
            profileData.BankDetails ??= new BankDetails();
            if (request.AccountNumber is not null)
                profileData.BankDetails.AccountNumber = request.AccountNumber;

            if (request.BankName is not null)
                profileData.BankDetails.BankName = request.BankName;

            if (request.AccountHolderName is not null)
                profileData.BankDetails.AccountHolderName =
                    request.AccountHolderName;
        }

        await context.SaveChangesAsync();

        logger.LogInformation(
            "Updated Profile data. User Id {@userId}. Response: Success",
            id
        );

        return Result<string?>.Success("Profile updated Successfully");
    }
}

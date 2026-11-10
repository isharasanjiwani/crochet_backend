using System;
using CrochetWorldApi.Data;
using CrochetWorldApi.Dtos.Common.Response;
using CrochetWorldApi.Dtos.General.Response;
using Microsoft.EntityFrameworkCore;

namespace CrochetWorldApi.Services.General;

public class GeneralService(AppDbContext context, ILogger<GeneralService> logger) : IGeneralService
{
    public async Task<Result<InitialDataResDto?>> GetInitialData(Guid id)
    {
        var initialData = await context.Users
            .Where(u => u.Id == id)
            .Select(u => new InitialDataResDto
            {
                Name = u.Name,
                ProfileUrl = u.ProfileImage,
                NotificationCount = u.Notifications.Select(n => !n.IsRead).Count()
            })
            .FirstOrDefaultAsync();

        if(initialData is null)
        {
            logger.LogWarning(
                "Get Initial Data Faild. UserId {@userId}. Response: Data not Found",
                id
            );
            return Result<InitialDataResDto?>.Failure("Initial Data not Found", ErrorType.NotFound);
        }

        logger.LogWarning(
            "Get Initial Data Success. UserId {@userId}.",
            id
        );
        return Result<InitialDataResDto?>.Success(initialData);
    }
}

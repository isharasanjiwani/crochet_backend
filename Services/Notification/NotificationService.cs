using System;
using CrochetWorldApi.Data;
using CrochetWorldApi.Dtos.Common.Response;
using CrochetWorldApi.Dtos.Notification.Response;
using Microsoft.EntityFrameworkCore;

namespace CrochetWorldApi.Services.Notification;

public class NotificationService(AppDbContext context, ILogger<NotificationService> logger) : INotificationService
{
    public async Task<Result<List<NotificationData>?>> GetNotificationList(Guid id)
    {
        var notificationList = await context.Notifications
            .Where(n => n.UserId == id)
            .Select(n => new NotificationData
            {
                Id = n.Id,
                Title = n.Title,
                Description = n.Description,
                Type = n.Type,
                IsRead = n.IsRead
            })
            .ToListAsync();

        if(notificationList is null)
        {
            logger.LogWarning(
                "Get Notification List Faild. UserId {@userId}. Response: Notifications not Found",
                id
            );
            return Result<List<NotificationData>?>.Failure("Notifications not Found", ErrorType.NotFound);
        }

        logger.LogWarning(
            "Get Notification List Success. UserId {@userId}. Notifications Count {@count}",
            id,
            notificationList.Count
        );
        return Result<List<NotificationData>?>.Success(notificationList);
    }

    public async Task<Result<string?>> NotificationMarkAsRead(Guid id, Guid notificationId)
    {
        var notification = await context.Notifications
            .Where(n => n.Id == notificationId && n.UserId == id)
            .FirstOrDefaultAsync();

        if(notification is null)
        {
            logger.LogWarning(
                "Mark Notification as Read. UserId {@userId}. Notification Id: {@notificationId} Response: Faild. Notifications not Found",
                id,
                notificationId
            );
            return Result<string?>.Failure("Notification not Found", ErrorType.NotFound);
        }

        notification.IsRead = true;


        logger.LogWarning(
            "Mark Notification as Read. UserId {@userId}. Notification Id: {@notificationId} Response: Success",
            id,
            notificationId
        );
        return Result<string?>.Success("Notification mark as Read Successfully");
    }
}

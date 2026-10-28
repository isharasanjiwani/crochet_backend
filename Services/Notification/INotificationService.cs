using System;
using CrochetWorldApi.Dtos.Common.Response;
using CrochetWorldApi.Dtos.Notification.Response;

namespace CrochetWorldApi.Services.Notification;

public interface INotificationService
{
    public Task<Result<List<NotificationData>?>> GetNotificationList(Guid id);
    public Task<Result<string?>> NotificationMarkAsRead(Guid id, Guid notificationId);
}

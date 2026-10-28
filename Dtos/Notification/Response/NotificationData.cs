using System;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CrochetWorldApi.Dtos.Notification.Response;

public class NotificationData
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Type { get; set; }
    public required bool IsRead { get; set; }
}

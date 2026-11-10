using System;

namespace CrochetWorldApi.Dtos.General.Response;

public class InitialDataResDto
{
    public required string Name { get; set; }
    public string? ProfileUrl { get; set; }
    public required int NotificationCount { get; set; }  
}

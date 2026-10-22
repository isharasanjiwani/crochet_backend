using System;

namespace CrochetWorldApi.Dtos.Auth.Responses;

public class UserResponseDto
{
    public required Guid UserId { get; set; }
    public required string Role { get; set; }
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}

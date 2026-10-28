using System;
using CrochetWorldApi.Enums;

namespace CrochetWorldApi.Dtos.Auth.Responses;

public class UserResponseDto
{
    public required Guid UserId { get; set; }
    public required UserType UserType { get; set; }
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}

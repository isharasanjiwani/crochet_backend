using System;
using System.ComponentModel.DataAnnotations;

namespace CrochetWorldApi.Dtos.Auth.Requests;

public class RefreshTokenRequestDto
{
    public Guid UserId { get; set; }

    [Required]
    public required string RefreshToken { get; set; }
}

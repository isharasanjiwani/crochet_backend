using System;
using System.ComponentModel.DataAnnotations;

namespace CrochetWorldApi.Dtos.Auth.Requests;

public class LoginUserRequestDto
{
    [Required]
    public required string Email {get; set;}

    [Required]
    public required string Password {get; set;}

    [Required]
    public required string Type {get; set;}
}

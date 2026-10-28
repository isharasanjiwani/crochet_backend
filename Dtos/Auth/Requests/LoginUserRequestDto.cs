using System;
using System.ComponentModel.DataAnnotations;
using CrochetWorldApi.Enums;

namespace CrochetWorldApi.Dtos.Auth.Requests;

public class LoginUserRequestDto
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public required string Email {get; set;}

    [Required]
    public required string Password {get; set;}

    [Required]
    public required UserType UserType {get; set;}
}

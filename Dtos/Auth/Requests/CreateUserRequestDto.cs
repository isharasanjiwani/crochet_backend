using System.ComponentModel.DataAnnotations;
using CrochetWorldApi.Enums;

namespace CrochetWorldApi.Dtos.Auth.Requests;

public class CreateUserRequestDto
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(50, MinimumLength = 3)]
    public required string Name { get; set; }

    [StringLength(50, MinimumLength = 3)]
    public string? ShopName { get; set; }

    [Required(ErrorMessage = "User type is required.")]
    public required UserType UserType { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public required string Email { get; set; }

    public string? Location { get; set; }

    [Phone]
    public string? MobileNumber { get; set; }

    public List<string>? ShopTypes { get; set; }

    public List<string>? PaymentMethods { get; set; }

    public string? AccountNumber { get; set; }

    public string? BankName { get; set; }

    public string? AccountHolderName { get; set; }

    [Required]
    public required string Password { get; set; }

}

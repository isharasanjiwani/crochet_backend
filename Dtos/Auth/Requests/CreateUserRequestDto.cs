using System.ComponentModel.DataAnnotations;

namespace CrochetWorldApi.Dtos.Auth.Requests;

public class CreateUserRequestDto
{
    [Required]
    public required string Name { get; set; }

    public string? ShopName { get; set; }

    [Required]
    public required string Role { get; set; }

    [Required]
    public required string Email { get; set; }

    public string? Location { get; set; }

    public string? MobileNumber { get; set; }

    public List<string>? ShopTypes { get; set; }

    public List<string>? PaymentMethods { get; set; }

    public string? AccountNumber { get; set; }

    public string? BankName { get; set; }

    public string? AccountHolderName { get; set; }

    [Required]
    public required string Password { get; set; }

}

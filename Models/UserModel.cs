using System.ComponentModel.DataAnnotations.Schema;

namespace CrochetWorldApi.Models;

[Table("users")]
public class UserModel : BaseModel
{
    public Guid Id { get; set; }
    public required string Type { get; set; }
    public required string Email { get; set; }
    public string? Location { get; set; }
    public string? MobileNumber { get; set; }
    public List<string>? ShopTypes { get; set; }
    public List<string>? PaymentMethods { get; set; }
    public BankDetails? BankDetails { get; set; }
    public required string Password { get; set; }
    public string? ProfileImage { get; set; }
}
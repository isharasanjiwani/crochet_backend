using System.ComponentModel.DataAnnotations.Schema;

namespace CrochetWorldApi.Entities;

[Table("users")]
public class UserEntity : BaseEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? ShopName { get; set; }
    public required string Role { get; set; }
    public required string Email { get; set; }
    public string? Location { get; set; }
    public string? MobileNumber { get; set; }
    public List<string>? ShopTypes { get; set; }
    public List<string>? PaymentMethods { get; set; }
    public BankDetails? BankDetails { get; set; }
    public required string Password { get; set; }
    public string? ProfileImage { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    public List<ProductEntity> Products { get; set; } = new List<ProductEntity>();

    public List<ProductReviewEntity> ProductReviews { get; set; } = new List<ProductReviewEntity>();

    public ICollection<OrderEntity> BuyerOrders { get; set; }
        = new List<OrderEntity>();
}
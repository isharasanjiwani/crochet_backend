using System.ComponentModel.DataAnnotations.Schema;

namespace CrochetWorldApi.Entities;

[Table("products")]
public class ProductEntity : BaseEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = null!;
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Material { get; set; }
    public required bool IsHandMade { get; set; }
    public required List<string> Images { get; set; }
    public required string Type { get; set; }
    public required int Quantity { get; set; }
    public List<string>? Colors { get; set; }
    public List<string>? Sizes { get; set; }
    public decimal? Price { get; set; }
    public List<ProductSpecification>? Specifications { get; set; }
    public required decimal DeliveryFee { get; set; }
    public required int RatingCount { get; set; }
    public required decimal AverageRating { get; set; }
    public bool IsAvailable { get; set; }
    public ICollection<ProductReviewEntity> Reviews { get; set; } = new List<ProductReviewEntity>();
    public ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
}
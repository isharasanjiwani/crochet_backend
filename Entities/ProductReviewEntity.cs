using System.ComponentModel.DataAnnotations.Schema;

namespace CrochetWorldApi.Entities;

[Table("product_reviews")]
public class ProductReviewEntity : BaseEntity
{
    public Guid Id { get; set; }
    public required int UserId { get; set; }
    public required int ProductId { get; set; }
    public ProductEntity Product { get; set; } = null!;
    public bool IsLiked { get; set; }
    public string? Comment { get; set; }
    public decimal? Rating { get; set; }
}
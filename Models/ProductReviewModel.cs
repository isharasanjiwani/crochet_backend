using System.ComponentModel.DataAnnotations.Schema;

namespace CrochetWorldApi.Models;

[Table("product_reviews")]
public class ProductReviewModel : BaseModel
{
    public Guid Id { get; set; }
    public required int UserId { get; set; }
    public required int ProductId { get; set; }
    public List<string>? Comments { get; set; }
    public decimal? Rating { get; set; }
}
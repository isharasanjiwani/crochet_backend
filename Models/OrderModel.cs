using System.ComponentModel.DataAnnotations.Schema;

namespace CrochetWorldApi.Models;

[Table("orders")]
public class OrderModel : BaseModel
{
    public Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public required Guid ProductId { get; set; }
    public required string MobileNumber { get; set; }
    public required string Address { get; set; }
    public required string PaymentMethod { get; set; }
    public string? Slip { get; set; }
    public required decimal TotalAmount { get; set; }
    public required List<OrderDetails> OrderDetails { get; set; }
    public required string Status { get; set; }
    public required DateTime EstimatedDelivery { get; set; }
}
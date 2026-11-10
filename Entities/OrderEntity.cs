using System.ComponentModel.DataAnnotations.Schema;
using CrochetWorldApi.Enums;

namespace CrochetWorldApi.Entities;

[Table("orders")]
public class OrderEntity : BaseEntity
{
    public Guid Id { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderNumber { get; set; }
    public required Guid UserId { get; set; }
    public UserEntity User { get; set; } = null!;
    public required Guid ProductId { get; set; }
    public ProductEntity Product { get; set; } = null!;
    public required int Quantity { get; set; }
    public required string MobileNumber { get; set; }
    public required string Address { get; set; }
    public required PaymentMethods PaymentMethod { get; set; }
    public string? Slip { get; set; }
    public required decimal TotalAmount { get; set; }
    public required List<OrderDetails> OrderDetails { get; set; }
    public required OrderStatus Status { get; set; }
    public DateTime EstimatedDelivery { get; set; }
}
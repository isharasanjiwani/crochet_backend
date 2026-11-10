using System;
using CrochetWorldApi.Enums;

namespace CrochetWorldApi.Dtos.Orders.Response;

public class SellerOrderDetailsRes
{
    public Guid Id { get; set; }
    public int OrderNumber { get; set; }
    public required int Quantity { get; set; }
    public required string BuyerName { get; set; } = null!;
    public string? BuyerProfileImage { get; set; }
    public required string MobileNumber { get; set; }
    public required string Address { get; set; }
    public required int ProductId { get; set; }
    public required string ProductName { get; set; }
    public required PaymentMethods PaymentMethod { get; set; }
    public string? Slip { get; set; }
    public required decimal TotalAmount { get; set; }
    public required List<OrderDetails> OrderDetails { get; set; }
    public required OrderStatus Status { get; set; }
}

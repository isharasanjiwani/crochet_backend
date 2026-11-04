using System;

namespace CrochetWorldApi.Dtos.Orders.Response;

public class BuyerOrderRes
{
    public Guid Id { get; set; }
    public int OrderNumber { get; set; }
    public string ProductName { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = null!;
    public required string ShopName { get; set; } = null!;
}

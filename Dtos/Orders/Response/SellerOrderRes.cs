using System;

namespace CrochetWorldApi.Dtos.Orders.Response;

public class SellerOrderRes
{
    public Guid Id { get; set; }
    public int OrderNumber { get; set; }
    public string ProductName { get; set; } = null!;
    public string ProductType { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = null!;
}

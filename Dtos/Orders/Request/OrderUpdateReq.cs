using System;
using CrochetWorldApi.Enums;

namespace CrochetWorldApi.Dtos.Orders.Request;

public class OrderUpdateReq
{
    public required Guid OrderId { get; set; }
    public required OrderStatus Status { get; set; }
    public string? TrackingNumber { get; set; }
    public DateTime? EstimatedDelivery { get; set; }
}

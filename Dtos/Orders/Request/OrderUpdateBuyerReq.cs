using System;
using CrochetWorldApi.Enums;

namespace CrochetWorldApi.Dtos.Orders.Request;

public class OrderUpdateBuyerReq
{
    public Guid OrderId { get; set; }
    public OrderStatus Status { get; set; }
}

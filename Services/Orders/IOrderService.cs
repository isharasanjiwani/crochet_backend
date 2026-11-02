using System;
using CrochetWorldApi.Dtos.Common.Response;
using CrochetWorldApi.Dtos.Orders.Request;
using CrochetWorldApi.Dtos.Orders.Response;
using CrochetWorldApi.Enums;
using CrochetWorldApi.Handlers;

namespace CrochetWorldApi.Services.Orders;

public interface IOrderService
{
    public Task<Result<string?>> CreateOrder(CreateOrderReq request, Guid userId);

    public Task<Result<PagedResult<SellerOrderRes>>> GetSellerOrders(OrderStatus? status, int pageNumber, int pageSize, Guid userId);
    
}

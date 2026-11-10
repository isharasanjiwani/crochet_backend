using System;
using CrochetWorldApi.Data;
using CrochetWorldApi.Dtos.Common.Response;
using CrochetWorldApi.Dtos.Orders.Request;
using CrochetWorldApi.Dtos.Orders.Response;
using CrochetWorldApi.Entities;
using CrochetWorldApi.Enums;
using CrochetWorldApi.Handlers;
using CrochetWorldApi.Services.Image;
using Microsoft.EntityFrameworkCore;

namespace CrochetWorldApi.Services.Orders;

public class OrderService(AppDbContext context, ILogger<OrderService> logger, IImageService imageService) : IOrderService
{
    public async Task<Result<string?>> CreateOrder(CreateOrderReq request, Guid userId)
    {

        if(request.PaymentMethod == PaymentMethods.BankTransfer && request.Slip == null)
        {
            logger.LogInformation(
                "Place Order. Product: {@userId}. Response: Failed. Reason: Slip is required for Bank Transfer payment method.",
                userId
            );
            return Result<string?>.Failure("Slip is required for Bank Transfer payment method.", ErrorType.Validation);
        }

        var availableCount = context.Products.Find(request.ProductId)?.Quantity;

        if (availableCount == null || availableCount < request.Quantity)
        {
            logger.LogInformation(
                "Place Order. Product: {@userId}. Response: Failed. Reason: Insufficient product quantity.",
                userId
            );
            return Result<string?>.Failure("Insufficient product quantity.", ErrorType.Validation);
        }

        var imageData = new ImageData { Url = string.Empty, Id = Guid.Empty };

        if(request.Slip != null)
        {
            imageData = await imageService.SaveImageAsync(request.Slip);
        }

        var order = new OrderEntity
        {
            ProductId = request.ProductId,
            UserId = userId,
            Quantity = request.Quantity,
            MobileNumber = request.MobileNumber,
            Address = request.Address,
            PaymentMethod = request.PaymentMethod, 
            Slip = imageData.Url,
            TotalAmount = request.TotalAmount,
            OrderDetails = request.OrderDetails,
            Status = OrderStatus.Pending,
        };

        context.Orders.Add(order);
        await context.SaveChangesAsync();

        logger.LogInformation(
            "Order Created Successfully. User: {@userId}. Response: Success. OrderId: {@orderId}",
            userId,
            order.Id
        );

        return Result<string?>.Success("Order created Successfully");
    }

    public async Task<Result<PagedResult<SellerOrderRes>>> GetSellerOrders(OrderStatus? status, int pageNumber, int pageSize, Guid userId)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
        {
            return Result<PagedResult<SellerOrderRes>>.Failure(
                "Invalid pagination parameters.",
                ErrorType.Validation
            );
        }

        var query = context.Orders.AsNoTracking();

        if (status != null)
        {
            query = query.Where(o => o.Status == status);
        }

        var totalCount = await query.CountAsync();

        var orders = await query
            .OrderBy(o => o.CreatedAt)
            .ThenBy(o => o.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(o => new SellerOrderRes
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                ProductName = o.Product.Name,
                ProductType = o.Product.Type,
                Quantity = o.Quantity,
                TotalAmount = o.TotalAmount,
                Status = o.Status.ToString()
            })
            .ToListAsync();

        if(totalCount == 0)
        {
            logger.LogWarning(
                "Get order Data. User: {@UserId} Response: Orders not Found",
                userId
            );
            return Result<PagedResult<SellerOrderRes>>.Failure(
                "Orders not Found", 
                ErrorType.NotFound
            );
        } 
        else
        {
            logger.LogWarning(
                "Get order Data. User: {@UserId} Response: Orders count: {@totalCount}",
                userId,
                totalCount
            );
            return Result<PagedResult<SellerOrderRes>>.Success(
                new PagedResult<SellerOrderRes>
                {
                    Items = orders,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                }
            );
        }
    }
}

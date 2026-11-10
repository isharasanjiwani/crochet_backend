using System.Security.Claims;
using CrochetWorldApi.Dtos.Common.Response;
using CrochetWorldApi.Dtos.Orders.Request;
using CrochetWorldApi.Dtos.Orders.Response;
using CrochetWorldApi.Enums;
using CrochetWorldApi.Handlers;
using CrochetWorldApi.Services.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrochetWorldApi.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController(IOrderService orderService) : BaseApiController
    {
        [HttpGet("/seller")]
        [Authorize(Roles = "Seller")]
        public async Task<ActionResult> GetOrdersBySeller([FromQuery] OrderStatus? status = null, int pageNumber = 1, int pageSize = 10)
        {
            var stringValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool success = Guid.TryParse(stringValue, out Guid userId);
            if(!success)
            {
                return Unauthorized();
            }
            var result = await orderService.GetSellerOrders(status, pageNumber, pageSize, userId);

            if(!result.IsSuccess)
                return HandleResult(result);

            return Ok(result.Data);
        }

        [HttpGet(":id/seller")]
        [Authorize(Roles = "Seller")]
        public async Task<ActionResult> GetOrderDetailsBySeller(Guid Id)
        {
            var result = await orderService.GetOrderDetailsBySeller(Id);

            if(!result.IsSuccess)
                return HandleResult(result);

            return Ok(result.Data);
        }

        [HttpPost("/update-status")]
        [Authorize(Roles = "Seller")]
        public async Task<ActionResult> UpdateOrderStatusBySeller([FromBody] OrderUpdateReq request)
        {
            var result = await orderService.UpdateOrderStatusBySeller(request);

            if(!result.IsSuccess)
                return HandleResult(result);

            return Ok(result.Data);
        }

        [HttpGet("/buyer")]
        [Authorize(Roles = "Buyer")]
        public async Task<ActionResult> GetOrdersByBuyer([FromQuery] OrderStatus? status = null, int pageNumber = 1, int pageSize = 10)
        {
            var stringValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool success = Guid.TryParse(stringValue, out Guid userId);
            if(!success)
            {
                return Unauthorized();
            }
            var result = await orderService.GetOrdersByBuyer(status, pageNumber, pageSize, userId);

            if(!result.IsSuccess)
                return HandleResult(result);

            return Ok(result.Data);
        }

        [HttpPost("buyer/update-status")]
        [Authorize(Roles = "Buyer")]
        public async Task<ActionResult> UpdateOrderStatusByBuyer(OrderUpdateBuyerReq request)
        {
            var stringValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool success = Guid.TryParse(stringValue, out Guid userId);
            if(!success)
            {
                return Unauthorized();
            }
            
            var result = await orderService.UpdateOrderStatusByBuyer(request, userId);

            if(!result.IsSuccess)
                return HandleResult(result);

            return Ok(result.Data);
        }
    }
}

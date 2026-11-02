using System.Security.Claims;
using CrochetWorldApi.Dtos.Common.Response;
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
    public class OrderController(IOrderService orderService) : ControllerBase
    {
        [HttpGet("/seller")]
        [Authorize(Roles = "Seller")]
        public async Task<ActionResult> GetSellerOrders([FromQuery] OrderStatus? status = null, int pageNumber = 1, [FromQuery] int pageSize = 10)
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

        private ActionResult HandleResult(Result<PagedResult<SellerOrderRes>> result)
        {
            throw new NotImplementedException();
        }
    }
}

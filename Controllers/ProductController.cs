using System.Security.Claims;
using CrochetWorldApi.Dtos.Common.Response;
using CrochetWorldApi.Dtos.Product.Request;
using CrochetWorldApi.Services.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CrochetWorldApi.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController(IProductService productService) : BaseApiController
    {
        [HttpPost]
        [Authorize(Roles = "Seller")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> CreateProduct([FromForm] CreateProductReqDto request)
        {
            var stringValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool success = Guid.TryParse(stringValue, out Guid userId);
            if(!success)
            {
                return Unauthorized();
            }
            var result = await productService.CreateProduct(request, userId);

            if(!result.IsSuccess)
                return HandleResult(result);

            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var stringValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool success = Guid.TryParse(stringValue, out Guid userId);
            if(!success)
            {
                return Unauthorized();
            }
            var result = await productService.GetProducts(pageNumber, pageSize, userId);

            if(!result.IsSuccess)
                return HandleResult(result);

            return Ok(result.Data);
        }

        [HttpGet("details/:id")]
        [Authorize]
        public async Task<ActionResult> GetProductDetailsById(Guid id)
        {
            var result = await productService.GetProductDetailsById(id);

            if(!result.IsSuccess)
                return HandleResult(result);

            return Ok(result.Data);
        }
    }
}

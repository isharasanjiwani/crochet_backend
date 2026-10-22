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
        public async Task<ActionResult> CreateProduct(CreateProductReqDto request)
        {
            var result = await productService.CreateProduct(request);

            if(!result.IsSuccess)
                return HandleResult(result);

            return Ok(result.Data);
        }
    }
}

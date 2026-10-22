using CrochetWorldApi.Dtos.Common.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrochetWorldApi.Controllers
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            return result.ErrorType switch
            {
                ErrorType.Validation =>
                    BadRequest(new { message = result.Error }),

                ErrorType.NotFound =>
                    NotFound(new { message = result.Error }),

                ErrorType.Conflict =>
                    Conflict(new { message = result.Error }),

                ErrorType.Unauthorized =>
                    Unauthorized(new { message = result.Error }),

                ErrorType.Forbidden =>
                    StatusCode(403, new { message = result.Error }),

                _ => StatusCode(500, new
                {
                    message = "An unexpected error occurred."
                })
            };
        }
    }
}

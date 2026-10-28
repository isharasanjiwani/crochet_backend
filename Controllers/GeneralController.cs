using System.Security.Claims;
using CrochetWorldApi.Services.General;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrochetWorldApi.Controllers
{
    [Route("api/general")]
    [ApiController]
    public class GeneralController(IGeneralService generalService) : BaseApiController
    {
        [HttpGet("/initial")]
        [Authorize]
        public async Task<ActionResult> GetInitialData()
        {
            var stringValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool success = Guid.TryParse(stringValue, out Guid userId);
            if(!success)
            {
                return Unauthorized();
            }
            var result = await generalService.GetInitialData(userId);

            if(!result.IsSuccess)
                return HandleResult(result);

            return Ok(result);
        }
    }
}

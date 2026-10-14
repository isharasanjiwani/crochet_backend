using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrochetWorldApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [HttpGet("test")]
        public Task<string> testApi()
        {
            return Task.FromResult("Success");
        }
    }
}

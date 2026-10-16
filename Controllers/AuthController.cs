using CrochetWorldApi.Dtos.Auth.Requests;
using CrochetWorldApi.Dtos.Auth.Responses;
using CrochetWorldApi.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrochetWorldApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _AuthService;

        public AuthController(IAuthService authService)
        {
            _AuthService = authService;
        }


        [HttpGet("test")]
        public Task<string> testApi()
        {
            return Task.FromResult("Success");
        }
        

        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDto>> Register(CreateUserRequestDto request)
        {
            var response = await _AuthService.CreateUserAsync(request);

            if(response is null)
                return BadRequest("Username already exists.");

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserResponseDto>> Login(LoginUserRequestDto request)
        {
            var response = await _AuthService.LoginUserAsync(request);
            if(response is null)
                return BadRequest("Invalid username or Password.");

            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<UserResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var response = await _AuthService.RefreshTokenAsync(request);
            if(response is null || response.AccessToken is null || response.RefreshToken is null)
                return Unauthorized("Invalid refresh token .");

            return Ok(response);
        }

        [Authorize(Roles = "Buyer")]
        [HttpGet("edit-profile")]
        public ActionResult<string> EditProfile()
        {
            return Ok("success");
        }
    }
}

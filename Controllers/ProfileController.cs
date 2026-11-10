using System.Security.Claims;
using CrochetWorldApi.Dtos.Profile.Requests;
using CrochetWorldApi.Services.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrochetWorldApi.Controllers
{
    [Route("api/profile")]
    [ApiController]
    public class ProfileController(IProfileService profileService) : BaseApiController
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetProfileData()
        {
            var stringValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool success = Guid.TryParse(stringValue, out Guid userId);
            if(!success)
            {
                return Unauthorized();
            }
            var result = await profileService.GetProfileData(userId);

            if(!result.IsSuccess)
                return HandleResult(result);

            return Ok(result);
        }

        [HttpPost("/edit")]
        [Authorize]
        public async Task<ActionResult> EditProfileData(EditProfileReqDto request)
        {
            var stringValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool success = Guid.TryParse(stringValue, out Guid userId);
            if(!success)
            {
                return Unauthorized();
            }
            var result = await profileService.EditProfileData(request, userId);

            if(!result.IsSuccess)
                return HandleResult(result);

            return Ok(result);
        }
    }
}

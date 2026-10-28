using System.Security.Claims;
using CrochetWorldApi.Services.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrochetWorldApi.Controllers
{
    [Route("api/notification")]
    [ApiController]
    public class NotificationController(INotificationService notificationService) : BaseApiController
    {
        
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetNotificationList()
        {
            var stringValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool success = Guid.TryParse(stringValue, out Guid userId);
            if(!success)
            {
                return Unauthorized();
            }
            var result = await notificationService.GetNotificationList(userId);

            if(!result.IsSuccess)
                return HandleResult(result);

            return Ok(result);
        }

        [HttpGet("/:id/mark")]
        [Authorize]
        public async Task<ActionResult> NotificationMarkAsRead(Guid notificationId)
        {
            var stringValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool success = Guid.TryParse(stringValue, out Guid userId);
            if(!success)
            {
                return Unauthorized();
            }
            var result = await notificationService.NotificationMarkAsRead(userId, notificationId);

            if(!result.IsSuccess)
                return HandleResult(result);

            return Ok(result);
        }
    }
}

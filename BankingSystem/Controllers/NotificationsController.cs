using Banking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("{userId}")]
        public IActionResult GetUserNotifications(int userId)
        {
            var notifications = _notificationService.GetUserNotifications(userId);
            return Ok(notifications);
        }

        [HttpPost("mark-as-read/{notificationId}")]
        public IActionResult MarkAsRead(int notificationId)
        {
            _notificationService.MarkAsRead(notificationId);
            return Ok();
        }
    }
}

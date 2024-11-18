using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTheater.Dtos.Notification;
using MovieTheater.Service.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTheater.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        [Authorize(Roles = "Customer, Admin")]
        public async Task<IActionResult> GetUserNotifications()
        {
            // Lấy userId từ token (JWT)
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value);

            var notifications = await _notificationService.GetUserNotificationsAsync(userId);
            return Ok(notifications);
        }

        [HttpPut("mark-as-read/{notificationId}")]
        [Authorize(Roles = "Customer, Admin")]
        public async Task<IActionResult> MarkNotificationAsRead(int notificationId)
        {
            // Lấy userId từ token (JWT)
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value);

            await _notificationService.MarkNotificationAsReadAsync(userId, notificationId);
            return NoContent();
        }
    }
}

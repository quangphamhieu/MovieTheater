using Microsoft.EntityFrameworkCore;
using MovieTheater.DbContexts;
using MovieTheater.Entities;
using MovieTheater.Dtos.Notification;
using MovieTheater.Service.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTheater.Service.Implement
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddNotificationAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }
        public async Task SendNotificationAsync(int userId, Notification notification)
        {
            var userNotification = new UserNotification
            {
                UserId = userId,
                NotificationId = notification.Id,
                IsRead = false,
                DateReceived = DateTime.Now
            };

            _context.UserNotifications.Add(userNotification);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserNotificationDto>> GetUserNotificationsAsync(int userId)
        {
            var userNotifications = await _context.UserNotifications
                .Where(un => un.UserId == userId)
                .Include(un => un.Notification)
                .OrderByDescending(un => un.Notification.CreatedAt)
                .ToListAsync();

            var userNotificationDtos = new List<UserNotificationDto>();

            foreach (var userNotification in userNotifications)
            {
                userNotificationDtos.Add(new UserNotificationDto
                {
                    Id = userNotification.Id,
                    UserId = userNotification.UserId,
                    NotificationId = userNotification.NotificationId,
                    IsRead = userNotification.IsRead,
                    DateReceived = userNotification.DateReceived,
                    NotificationTitle = userNotification.Notification.Title,
                    NotificationMessage = userNotification.Notification.Message
                });
            }

            return userNotificationDtos;
        }

        public async Task MarkNotificationAsReadAsync(int userId, int notificationId)
        {
            var userNotification = await _context.UserNotifications
                .FirstOrDefaultAsync(un => un.UserId == userId && un.NotificationId == notificationId);

            if (userNotification != null)
            {
                userNotification.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}

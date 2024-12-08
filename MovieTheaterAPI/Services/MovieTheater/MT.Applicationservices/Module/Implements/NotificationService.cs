using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MT.Applicationservices.Common;
using MT.Applicationservices.Module.Abstracts;
using MT.Domain;
using MT.Dtos.Notification;
using MT.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Applicationservices.Module.Implements
{
    public class NotificationService : MovieTheaterBaseService, INotificationService
    {
        public NotificationService(ILogger<NotificationService> logger, MovieTheaterDbContext dbContext)
        : base(logger, dbContext)
        {
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            _logger.LogInformation("Adding a new notification.");
            _dbContext.Notifications.Add(notification);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SendNotificationAsync(int userId, Notification notification)
        {
            _logger.LogInformation($"Sending notification {notification.Id} to user {userId}.");

            var userNotification = new UserNotification
            {
                UserId = userId,
                NotificationId = notification.Id,
                IsRead = false,
                DateReceived = DateTime.Now
            };

            _dbContext.UserNotifications.Add(userNotification);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<UserNotificationDto>> GetUserNotificationsAsync(int userId)
        {
            _logger.LogInformation($"Fetching notifications for user {userId}.");
            var userNotifications = await _dbContext.UserNotifications
                .Where(un => un.UserId == userId)
                .Include(un => un.Notification)
                .OrderByDescending(un => un.Notification.CreatedAt)
                .ToListAsync();

            return userNotifications.Select(un => new UserNotificationDto
            {
                Id = un.Id,
                UserId = un.UserId,
                NotificationId = un.NotificationId,
                IsRead = un.IsRead,
                DateReceived = un.DateReceived,
                NotificationTitle = un.Notification.Title,
                NotificationMessage = un.Notification.Message
            }).ToList();
        }

        public async Task MarkNotificationAsReadAsync(int userId, int notificationId)
        {
            _logger.LogInformation($"Marking notification {notificationId} as read for user {userId}.");
            var userNotification = await _dbContext.UserNotifications
                .FirstOrDefaultAsync(un => un.UserId == userId && un.NotificationId == notificationId);

            if (userNotification != null)
            {
                userNotification.IsRead = true;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning($"Notification {notificationId} not found for user {userId}.");
            }
        }
    }
}

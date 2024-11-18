using MovieTheater.Dtos.Notification;
using MovieTheater.Entities;

namespace MovieTheater.Service.Abstract
{
    public interface INotificationService
    {
        Task AddNotificationAsync(Notification notification);
        Task SendNotificationAsync(int userId, Notification notification); // Gửi thông báo
        Task<List<UserNotificationDto>> GetUserNotificationsAsync(int userId);   // Lấy thông báo của người dùng
        Task MarkNotificationAsReadAsync(int userId, int notificationId); // Đánh dấu thông báo đã đọc
    }

}

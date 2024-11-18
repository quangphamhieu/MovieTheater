using System;

namespace MovieTheater.Entities
{
    public enum NotificationType
    {
        NewMovie,
        MovieRemoved
    }

    public class Notification
    {
        public int Id { get; set; }                // ID thông báo
        public string Title { get; set; }          // Tiêu đề thông báo
        public string Message { get; set; }        // Nội dung thông báo
        public NotificationType Type { get; set; }  // Loại thông báo (Info, Warning, Error, Success)
        public DateTime CreatedAt { get; set; }    // Thời gian tạo thông báo
        public bool IsActive { get; set; }         // Trạng thái hoạt động của thông báo (có thể vô hiệu hóa)
    }
}

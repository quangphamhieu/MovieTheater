namespace MovieTheater.Dtos.Notification
{
    public class CreateNotificationDto
    {
        public string Title { get; set; }  // Tiêu đề thông báo
        public string Message { get; set; } // Nội dung thông báo
        public string Type { get; set; } // Loại thông báo
    }
}

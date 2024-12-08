using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Dtos.Notification
{
    public class UserNotificationDto
    {
        public int Id { get; set; }  // Thêm Id vào DTO nếu cần
        public int UserId { get; set; } // Thêm UserId vào DTO
        public int NotificationId { get; set; }
        public string NotificationTitle { get; set; }
        public string NotificationMessage { get; set; }
        public bool IsRead { get; set; }
        public DateTime DateReceived { get; set; }
    }
}

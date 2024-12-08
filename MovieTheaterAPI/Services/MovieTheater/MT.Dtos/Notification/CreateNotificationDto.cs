using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Dtos.Notification
{
    public class CreateNotificationDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tiêu đề không được bỏ trống")]
        [MaxLength(255, ErrorMessage = "Tiêu đề không được vượt quá 255 ký tự")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Nội dung không được bỏ trống")]
        public string Message { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Loại thông báo không được bỏ trống")]
        public string Type { get; set; }
    }
}

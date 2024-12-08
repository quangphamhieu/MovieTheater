using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Dtos.Comment
{
    public class UpdateCommentDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Nội dung không được bỏ trống")]
        [MaxLength(1000, ErrorMessage = "Nội dung không được vượt quá 1000 ký tự")]
        public string Content { get; set; }

        [Range(1, 10, ErrorMessage = "Rating phải nằm trong khoảng 1 đến 10")]
        public int Rating { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Dtos.Director
{
    public class CreateDirectorDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tên đạo diễn không được bỏ trống")]
        [MaxLength(255, ErrorMessage = "Tên đạo diễn không được vượt quá 255 ký tự")]
        public string Name { get; set; }
    }
}

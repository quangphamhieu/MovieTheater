using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Dtos.Genre
{
    public class CreateGenreDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tên thể loại không được bỏ trống")]
        [MaxLength(255, ErrorMessage = "Tên thể loại không được vượt quá 255 ký tự")]
        public string Name { get; set; }
    }
}

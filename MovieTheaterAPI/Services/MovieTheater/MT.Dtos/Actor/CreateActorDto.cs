using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Dtos.Actor
{
    public class CreateActorDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tên không được bỏ trống")]
        [MaxLength(255, ErrorMessage = "Tên không được vượt quá 255 ký tự")]
        public string Name { get; set; }
    }
}

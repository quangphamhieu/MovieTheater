using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Dtos.Role
{
    public class CreateRoleDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tên vai trò không được bỏ trống")]
        [MaxLength(255, ErrorMessage = "Tên vai trò không được vượt quá 255 ký tự")]
        public string RoleName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Dtos.User
{
    public class CreateUserDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Họ tên không được bỏ trống")]
        public string FullName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email không được bỏ trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu không được bỏ trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string Password { get; set; }

        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        [Required(ErrorMessage = "Vai trò không được bỏ trống")]
        public int RoleId { get; set; }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MT.Applicationservices.Module.Abstracts;
using MT.Dtos.User;
using System;
using System.Threading.Tasks;

namespace MovieTheater.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Ensure the user is authenticated before accessing any of the endpoints
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Lấy tất cả người dùng - Only accessible by admin
        [HttpGet]
        [Authorize(Roles = "Admin")] // Restrict access to Admin only
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userService.GetAllAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Lấy người dùng theo Id
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")] // Admins can get user by ID
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user == null)
                    return NotFound("User not found.");

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Lấy người dùng theo Email
        [HttpGet("email/{email}")]
        [Authorize(Roles = "Admin")] // Admins can search user by email
        public async Task<IActionResult> GetByEmail(string email)
        {
            try
            {
                var user = await _userService.GetByEmailAsync(email);
                if (user == null)
                    return NotFound("User not found.");

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] CreateUserDto dto)
        {
            dto.Address = dto.Email;

            var user = await _userService.RegisterUserAsync(dto);

            if (user != null)
            {
                return Ok(new { message = "User registered successfully!" });
            }

            return BadRequest(new { message = "Registration failed." });
        }

        // Lấy thông tin người dùng hiện tại
        [HttpGet("current")]
        public IActionResult GetCurrentUser()
        {
            try
            {
                var user = _userService.FindUserById();
                if (user == null)
                    return NotFound("User not found");

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Cập nhật thông tin người dùng
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedUser = await _userService.UpdateAsync(id, dto);
                if (updatedUser == null)
                    return NotFound("User not found.");

                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Cập nhật mật khẩu người dùng
        [HttpPut("password/{id}")]
        public async Task<IActionResult> UpdatePassword(int id, [FromBody] UpdatePasswordDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var isUpdated = await _userService.UpdatePasswordAsync(id, dto);
                if (!isUpdated)
                    return BadRequest("Current password is incorrect or new password does not match.");

                return Ok("Password updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Đặt lại mật khẩu (Reset Password)
        [HttpPost("reset-password")]
        [AllowAnonymous] // Không yêu cầu xác thực cho endpoint này
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var isReset = await _userService.ResetPasswordAsync(dto);
                if (!isReset)
                    return BadRequest("Invalid token or email address.");

                return Ok("Password has been reset successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Xóa người dùng
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only Admin can delete users
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var isDeleted = await _userService.DeleteAsync(id);
                if (!isDeleted)
                    return NotFound("User not found.");

                return Ok("User deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTheater.Dtos.User;
using MovieTheater.Service.Abstract;
using System;

namespace MovieTheater.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  // Ensure the user is authenticated before accessing any of the endpoints
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
                // Log the error if needed
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
                // Log the error if needed
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
                // Log the error if needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Tạo người dùng mới
        [HttpPost("signup")]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdUser = await _userService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                // Log the error if needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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
                // Log the error if needed
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
                // Log the error if needed
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
                // Log the error if needed
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
                // Log the error if needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

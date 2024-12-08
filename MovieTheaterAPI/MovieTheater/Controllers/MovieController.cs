using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using MT.Applicationservices.Module.Abstracts;
using MT.Domain;
using MT.Dtos.Movie;

namespace MovieTheater.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Đảm bảo người dùng phải đăng nhập trước khi truy cập bất kỳ endpoint nào
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService; // Dịch vụ người dùng để hỗ trợ gửi thông báo

        public MovieController(
            IMovieService movieService,
            INotificationService notificationService,
            IUserService userService)
        {
            _movieService = movieService;
            _notificationService = notificationService;
            _userService = userService;
        }

        // Lấy danh sách tất cả phim
        [HttpGet]
        [Authorize(Roles = "Admin, Customer")] // Chỉ Admin hoặc Customer mới có thể xem danh sách phim
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var movies = await _movieService.GetAllAsync();
                return Ok(movies); // Trả về danh sách phim dưới dạng JSON
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Lấy thông tin phim theo ID
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Customer")] // Chỉ Admin hoặc Customer mới có thể xem phim theo ID
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var movie = await _movieService.GetByIdAsync(id);
                if (movie == null)
                    return NotFound("Movie not found.");

                return Ok(movie);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Tạo phim mới
        [HttpPost]
        [Authorize(Roles = "Admin")] // Chỉ Admin mới được tạo phim
        public async Task<IActionResult> Create([FromBody] CreateMovieDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdMovie = await _movieService.CreateAsync(dto);

                // Gửi thông báo về phim mới
                var notification = new Notification
                {
                    Title = "New Movie Released",
                    Message = $"A new movie '{createdMovie.Title}' has been added to the theater!",
                    Type = NotificationType.NewMovie,
                    CreatedAt = DateTime.Now
                };
                await _notificationService.AddNotificationAsync(notification);
                // Gửi thông báo đến tất cả người dùng
                var users = await _userService.GetAllAsync();
                foreach (var user in users)
                {
                    await _notificationService.SendNotificationAsync(user.Id, notification);
                }

                return CreatedAtAction(nameof(GetById), new { id = createdMovie.Id }, createdMovie);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Cập nhật phim
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Chỉ Admin mới được chỉnh sửa phim
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMovieDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedMovie = await _movieService.UpdateAsync(id, dto);
                if (updatedMovie == null)
                    return NotFound("Movie not found.");

                return Ok(updatedMovie);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Xóa phim
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Chỉ Admin mới được xóa phim
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var movie = await _movieService.GetByIdAsync(id);
                if (movie == null)
                    return NotFound("Movie not found.");

                var isDeleted = await _movieService.DeleteAsync(id);
                if (!isDeleted)
                    return BadRequest("Could not delete the movie.");

                // Gửi thông báo về việc xóa phim
                var notification = new Notification
                {
                    Title = "Movie Removed",
                    Message = $"The movie '{movie.Title}' has been removed from the theater.",
                    Type = NotificationType.MovieRemoved,
                    CreatedAt = DateTime.Now
                };
                await _notificationService.AddNotificationAsync(notification);
                // Gửi thông báo đến tất cả người dùng
                var users = await _userService.GetAllAsync();
                foreach (var user in users)
                {
                    await _notificationService.SendNotificationAsync(user.Id, notification);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

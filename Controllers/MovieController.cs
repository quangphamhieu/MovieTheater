using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTheater.Dtos.Movie;
using MovieTheater.Service.Abstract;
using MovieTheater.Service.Implement;
using System;

namespace MovieTheater.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        // Chỉ customer có thể xem tất cả các phim
        [HttpGet]
        [Authorize(Roles = "Customer, Admin")]
        public async Task<IActionResult> GetAllMovies()
        {
            try
            {
                var movies = await _movieService.GetAllAsync();
                return Ok(movies);
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần và trả về lỗi cho người dùng
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Chỉ customer có thể tìm phim theo ID
        [HttpGet("{id}")]
        [Authorize(Roles = "Customer, Admin")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            try
            {
                var movie = await _movieService.GetByIdAsync(id);
                if (movie == null)
                    return NotFound();

                return Ok(movie);
            }
            catch (Exception ex)
            {
                // Log lỗi và trả về lỗi cho người dùng
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Admin có quyền tạo phim mới
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateMovie([FromBody] CreateMovieDto dto)
        {
            try
            {
                var movie = await _movieService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
            }
            catch (Exception ex)
            {
                // Log lỗi và trả về lỗi cho người dùng
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchMoviesByTitle([FromQuery] string title)
        {
            try
            {
                // Chỉ cho phép khách hàng tìm kiếm phim theo tiêu đề
                var movies = await _movieService.SearchMoviesByTitleAsync(title);
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Admin có quyền cập nhật phim
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] UpdateMovieDto dto)
        {
            try
            {
                var movie = await _movieService.UpdateAsync(id, dto);
                if (movie == null)
                    return NotFound();

                return Ok(movie);
            }
            catch (Exception ex)
            {
                // Log lỗi và trả về lỗi cho người dùng
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Admin có quyền xóa phim
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            try
            {
                var result = await _movieService.DeleteAsync(id);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log lỗi và trả về lỗi cho người dùng
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

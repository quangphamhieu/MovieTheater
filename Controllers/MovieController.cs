using Microsoft.AspNetCore.Mvc;
using MovieTheater.Dtos.Movie;
using MovieTheater.Service.Abstract;
using MovieTheater.Service.Implement;

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

        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await _movieService.GetAllAsync();
            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await _movieService.GetByIdAsync(id);
            return movie == null ? NotFound() : Ok(movie);
        }

        [HttpGet("title/{title}")]
        public async Task<IActionResult> GetMovieByTitle(string title)
        {
            var movie = await _movieService.GetByTitleAsync(title);
            return movie == null ? NotFound() : Ok(movie);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromBody] CreateMovieDto dto)
        {
            var movie = await _movieService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] UpdateMovieDto dto)
        {
            var movie = await _movieService.UpdateAsync(id, dto);
            return movie == null ? NotFound() : Ok(movie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var result = await _movieService.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}

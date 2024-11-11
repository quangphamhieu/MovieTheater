using Microsoft.AspNetCore.Mvc;
using MovieTheater.Dtos.Genre;
using MovieTheater.Service.Abstract;
using MovieTheater.Service.Implement;

namespace MovieTheater.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGenres()
        {
            var genres = await _genreService.GetAllAsync();
            return Ok(genres);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGenreById(int id)
        {
            var genre = await _genreService.GetByIdAsync(id);
            return genre == null ? NotFound() : Ok(genre);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGenre([FromBody] CreateGenreDto dto)
        {
            var genre = await _genreService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetGenreById), new { id = genre.Id }, genre);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGenre(int id, [FromBody] UpdateGenreDto dto)
        {
            var genre = await _genreService.UpdateAsync(id, dto);
            return genre == null ? NotFound() : Ok(genre);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var result = await _genreService.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}

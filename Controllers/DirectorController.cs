using Microsoft.AspNetCore.Mvc;
using MovieTheater.Dtos.Director;
using MovieTheater.Service.Abstract;
using MovieTheater.Service.Implement;

namespace MovieTheater.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirectorsController : ControllerBase
    {
        private readonly IDirectorService _directorService;

        public DirectorsController(IDirectorService directorService)
        {
            _directorService = directorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDirectors()
        {
            var directors = await _directorService.GetAllAsync();
            return Ok(directors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDirectorById(int id)
        {
            var director = await _directorService.GetByIdAsync(id);
            return director == null ? NotFound() : Ok(director);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDirector([FromBody] CreateDirectorDto dto)
        {
            var director = await _directorService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetDirectorById), new { id = director.Id }, director);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDirector(int id, [FromBody] UpdateDirectorDto dto)
        {
            var director = await _directorService.UpdateAsync(id, dto);
            return director == null ? NotFound() : Ok(director);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDirector(int id)
        {
            var result = await _directorService.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}

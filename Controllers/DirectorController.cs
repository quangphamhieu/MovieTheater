using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTheater.Dtos.Director;
using MovieTheater.Service.Abstract;
using System;
using System.Threading.Tasks;

namespace MovieTheater.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] // Restrict access to Admin role only
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
            try
            {
                var directors = await _directorService.GetAllAsync();
                return Ok(directors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDirectorById(int id)
        {
            try
            {
                var director = await _directorService.GetByIdAsync(id);
                return director == null ? NotFound() : Ok(director);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDirector([FromBody] CreateDirectorDto dto)
        {
            try
            {
                var director = await _directorService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetDirectorById), new { id = director.Id }, director);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDirector(int id, [FromBody] UpdateDirectorDto dto)
        {
            try
            {
                var director = await _directorService.UpdateAsync(id, dto);
                return director == null ? NotFound() : Ok(director);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDirector(int id)
        {
            try
            {
                var result = await _directorService.DeleteAsync(id);
                return result ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}

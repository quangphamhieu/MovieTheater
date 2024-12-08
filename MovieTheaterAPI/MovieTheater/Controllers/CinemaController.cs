using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MT.Applicationservices.Module.Abstracts;
using MT.Dtos.Cinema;

namespace MovieTheater.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] // Restrict access to Admin role only
    public class CinemaController : ControllerBase
    {
        private readonly ICinemaService _cinemaService;

        public CinemaController(ICinemaService cinemaService)
        {
            _cinemaService = cinemaService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CinemaDto>>> GetAllCinemas()
        {
            try
            {
                var cinemas = await _cinemaService.GetAllCinemasAsync();
                return Ok(cinemas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CinemaDto>> GetCinemaById(int id)
        {
            try
            {
                var cinema = await _cinemaService.GetCinemaByIdAsync(id);
                return cinema == null ? NotFound() : Ok(cinema);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<CinemaDto>> CreateCinema(CreateCinemaDto createCinemaDto)
        {
            try
            {
                var cinema = await _cinemaService.CreateCinemaAsync(createCinemaDto);
                return CreatedAtAction(nameof(GetCinemaById), new { id = cinema.Id }, cinema);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CinemaDto>> UpdateCinema(int id, UpdateCinemaDto updateCinemaDto)
        {
            try
            {
                var cinema = await _cinemaService.UpdateCinemaAsync(id, updateCinemaDto);
                return cinema == null ? NotFound() : Ok(cinema);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCinema(int id)
        {
            try
            {
                var result = await _cinemaService.DeleteCinemaAsync(id);
                return result ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}

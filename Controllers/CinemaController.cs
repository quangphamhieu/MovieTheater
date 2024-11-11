using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieTheater.Dtos.Cinema;
using MovieTheater.Service.Implement;
using MovieTheater.Service.Abstract;

namespace MovieTheater.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            var cinemas = await _cinemaService.GetAllCinemasAsync();
            return Ok(cinemas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CinemaDto>> GetCinemaById(int id)
        {
            var cinema = await _cinemaService.GetCinemaByIdAsync(id);
            if (cinema == null) return NotFound();

            return Ok(cinema);
        }

        [HttpPost]
        public async Task<ActionResult<CinemaDto>> CreateCinema(CreateCinemaDto createCinemaDto)
        {
            var cinema = await _cinemaService.CreateCinemaAsync(createCinemaDto);
            return CreatedAtAction(nameof(GetCinemaById), new { id = cinema.Id }, cinema);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CinemaDto>> UpdateCinema(int id, UpdateCinema updateCinemaDto)
        {
            var cinema = await _cinemaService.UpdateCinemaAsync(id, updateCinemaDto);
            if (cinema == null) return NotFound();

            return Ok(cinema);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCinema(int id)
        {
            var result = await _cinemaService.DeleteCinemaAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using MovieTheater.Dtos.ShowTime;
using MovieTheater.Service.Implement;
using MovieTheater.Service.Abstract;
using System.Threading.Tasks;

namespace MovieTheater.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowTimeController : ControllerBase
    {
        private readonly IShowTimeService _showTimeService;

        public ShowTimeController(IShowTimeService showTimeService)
        {
            _showTimeService = showTimeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var showTimes = await _showTimeService.GetAllShowTimesAsync();
            return Ok(showTimes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var showTime = await _showTimeService.GetShowTimeByIdAsync(id);
            if (showTime == null) return NotFound();

            return Ok(showTime);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateShowTimeDto createShowTimeDto)
        {
            var showTime = await _showTimeService.CreateShowTimeAsync(createShowTimeDto);
            if (showTime == null) return BadRequest("Movie, Cinema, or CinemaRoom not found");

            return CreatedAtAction(nameof(GetById), new { id = showTime.Id }, showTime);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateShowTimeDto updateShowTimeDto)
        {
            var showTime = await _showTimeService.UpdateShowTimeAsync(id, updateShowTimeDto);
            if (showTime == null) return NotFound();

            return Ok(showTime);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _showTimeService.DeleteShowTimeAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}

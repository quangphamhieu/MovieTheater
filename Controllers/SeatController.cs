using Microsoft.AspNetCore.Mvc;
using MovieTheater.Service.Abstract;

namespace MovieTheater.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        [HttpPost("generate/{cinemaRoomId}")]
        public async Task<IActionResult> GenerateSeats(int cinemaRoomId)
        {
            var seats = await _seatService.GenerateSeatsAsync(cinemaRoomId);
            return Ok(seats);
        }

        [HttpGet("room/{cinemaRoomId}")]
        public async Task<IActionResult> GetSeats(int cinemaRoomId)
        {
            var seats = await _seatService.GetSeatsByRoomAsync(cinemaRoomId);
            return Ok(seats);
        }
    }
}

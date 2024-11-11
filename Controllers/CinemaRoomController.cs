using Microsoft.AspNetCore.Mvc;
using MovieTheater.Dtos.CinemaRoom;
using MovieTheater.Service.Implement;
using MovieTheater.Service.Abstract;
using System.Threading.Tasks;

namespace MovieTheater.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CinemaRoomController : ControllerBase
    {
        private readonly ICinemaRoomService _cinemaRoomService;

        public CinemaRoomController(ICinemaRoomService cinemaRoomService)
        {
            _cinemaRoomService = cinemaRoomService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cinemaRooms = await _cinemaRoomService.GetAllCinemaRoomsAsync();
            return Ok(cinemaRooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cinemaRoom = await _cinemaRoomService.GetCinemaRoomByIdAsync(id);
            if (cinemaRoom == null) return NotFound();

            return Ok(cinemaRoom);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCinemaRoomDto createCinemaRoomDto)
        {
            var cinemaRoom = await _cinemaRoomService.CreateCinemaRoomAsync(createCinemaRoomDto);
            if (cinemaRoom == null) return BadRequest("Cinema not found");

            return CreatedAtAction(nameof(GetById), new { id = cinemaRoom.Id }, cinemaRoom);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCinemaRoomDto updateCinemaRoomDto)
        {
            var cinemaRoom = await _cinemaRoomService.UpdateCinemaRoomAsync(id, updateCinemaRoomDto);
            if (cinemaRoom == null) return NotFound();

            return Ok(cinemaRoom);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _cinemaRoomService.DeleteCinemaRoomAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}

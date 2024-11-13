using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTheater.Dtos.CinemaRoom;
using MovieTheater.Service.Abstract;
using System;
using System.Threading.Tasks;

namespace MovieTheater.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Restrict access to Admin role only
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
            try
            {
                var cinemaRooms = await _cinemaRoomService.GetAllCinemaRoomsAsync();
                return Ok(cinemaRooms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var cinemaRoom = await _cinemaRoomService.GetCinemaRoomByIdAsync(id);
                return cinemaRoom == null ? NotFound() : Ok(cinemaRoom);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCinemaRoomDto createCinemaRoomDto)
        {
            try
            {
                var cinemaRoom = await _cinemaRoomService.CreateCinemaRoomAsync(createCinemaRoomDto);
                return cinemaRoom == null
                    ? BadRequest("Cinema not found")
                    : CreatedAtAction(nameof(GetById), new { id = cinemaRoom.Id }, cinemaRoom);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCinemaRoomDto updateCinemaRoomDto)
        {
            try
            {
                var cinemaRoom = await _cinemaRoomService.UpdateCinemaRoomAsync(id, updateCinemaRoomDto);
                return cinemaRoom == null ? NotFound() : Ok(cinemaRoom);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _cinemaRoomService.DeleteCinemaRoomAsync(id);
                return deleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}

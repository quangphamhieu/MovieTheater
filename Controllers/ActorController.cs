using Microsoft.AspNetCore.Mvc;
using MovieTheater.Dtos.Actor;
using MovieTheater.Service.Abstract;
using MovieTheater.Service.Implement;

namespace MovieTheater.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActorsController : ControllerBase
    {
        private readonly IActorService _actorService;

        public ActorsController(IActorService actorService)
        {
            _actorService = actorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActors()
        {
            var actors = await _actorService.GetAllAsync();
            return Ok(actors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetActorById(int id)
        {
            var actor = await _actorService.GetByIdAsync(id);
            return actor == null ? NotFound() : Ok(actor);
        }

        [HttpPost]
        public async Task<IActionResult> CreateActor([FromBody] CreateActorDto dto)
        {
            var actor = await _actorService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetActorById), new { id = actor.Id }, actor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateActor(int id, [FromBody] UpdateActorDto dto)
        {
            var actor = await _actorService.UpdateAsync(id, dto);
            return actor == null ? NotFound() : Ok(actor);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActor(int id)
        {
            var result = await _actorService.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}

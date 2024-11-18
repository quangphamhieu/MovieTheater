using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTheater.Dtos.Ticket;
using MovieTheater.Entities;
using MovieTheater.Service.Abstract;

namespace MovieTheater.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateTicket([FromBody] TicketCreateRequest request)
        {
            // Lấy userId từ token (JWT)
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value);

            var ticket = await _ticketService.CreateTicketAsync(request.ShowTimeId, request.SeatIds, userId);
            return Ok(ticket);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTickets()
        {
            var tickets = await _ticketService.GetAllTicketsAsync();
            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketById(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            return ticket != null ? Ok(ticket) : NotFound();
        }
    }
}

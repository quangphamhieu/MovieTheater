using MovieTheater.Dtos.Ticket;

namespace MovieTheater.Service.Abstract
{
    public interface ITicketService
    {
        Task<TicketDto> CreateTicketAsync(int showTimeId, List<int> seatIds, int userId);
        Task<List<TicketDto>> GetAllTicketsAsync();
        Task<TicketDto> GetTicketByIdAsync(int ticketId);
    }
}

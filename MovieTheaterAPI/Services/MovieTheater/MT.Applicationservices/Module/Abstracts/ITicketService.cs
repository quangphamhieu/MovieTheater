using MT.Dtos.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Applicationservices.Module.Abstracts
{
    public interface ITicketService
    {
        Task<TicketDto> CreateTicketAsync(TicketCreateRequest ticketCreateRequest, int userId);
        Task<List<TicketDto>> GetAllTicketsAsync();
        Task UpdateTicketStatusAsync();
        Task<TicketDto> GetTicketByIdAsync(int ticketId);
    }
}

using MT.Dtos.Seat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Applicationservices.Module.Abstracts
{
    public interface ISeatService
    {
        Task<List<SeatDto>> GenerateSeatsAsync(int cinemaRoomId);
        Task<List<SeatDto>> GetSeatsByRoomAsync(int cinemaRoomId);
    }
}

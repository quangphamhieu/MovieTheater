using MT.Dtos.CinemaRoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Applicationservices.Module.Abstracts
{
    public interface ICinemaRoomService
    {
        Task<IEnumerable<CinameRoomDto>> GetAllCinemaRoomsAsync();
        Task<CinameRoomDto> GetCinemaRoomByIdAsync(int id);
        Task<CinameRoomDto> CreateCinemaRoomAsync(CreateCinemaRoomDto createCinemaRoomDto);
        Task<CinameRoomDto> UpdateCinemaRoomAsync(int id, UpdateCinemaRoomDto updateCinemaRoomDto);
        Task<bool> DeleteCinemaRoomAsync(int id);
    }
}

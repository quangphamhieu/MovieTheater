using MovieTheater.Dtos.CinemaRoom;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTheater.Service.Implement
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

using MovieTheater.Dtos.Seat;

namespace MovieTheater.Service.Abstract
{
    public interface ISeatService
    {
        Task<List<SeatDto>> GenerateSeatsAsync(int cinemaRoomId);
        Task<List<SeatDto>> GetSeatsByRoomAsync(int cinemaRoomId);
    }
}

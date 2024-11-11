using MovieTheater.DbContexts;
using MovieTheater.Dtos.CinemaRoom;
using MovieTheater.Entities;
using Microsoft.EntityFrameworkCore;
using MovieTheater.Service.Implement;

namespace MovieTheater.Service.Abstract
{
    public class CinemaRoomService : ICinemaRoomService
    {
        private readonly ApplicationDbContext _context;

        public CinemaRoomService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CinameRoomDto>> GetAllCinemaRoomsAsync()
        {
            return await _context.CinemaRooms
                .Select(cr => new CinameRoomDto
                {
                    Id = cr.Id,
                    Name = cr.Name,
                    CinemaName = cr.Cinema.Name,
                    SeatRows = cr.SeatRows,
                    SeatColumns = cr.SeatColumns
                })
                .ToListAsync();
        }

        public async Task<CinameRoomDto> GetCinemaRoomByIdAsync(int id)
        {
            var cinemaRoom = await _context.CinemaRooms
                .Include(cr => cr.Cinema)
                .FirstOrDefaultAsync(cr => cr.Id == id);

            if (cinemaRoom == null) return null;

            return new CinameRoomDto
            {
                Id = cinemaRoom.Id,
                Name = cinemaRoom.Name,
                CinemaName = cinemaRoom.Cinema.Name,
                SeatRows = cinemaRoom.SeatRows,
                SeatColumns = cinemaRoom.SeatColumns
            };
        }

        public async Task<CinameRoomDto> CreateCinemaRoomAsync(CreateCinemaRoomDto createCinemaRoomDto)
        {
            var cinema = await _context.Cinemas.FirstOrDefaultAsync(c => c.Name == createCinemaRoomDto.CinemaName);
            if (cinema == null) return null;

            var cinemaRoom = new CinemaRoom
            {
                Name = createCinemaRoomDto.Name,
                CinemaId = cinema.Id,
                SeatRows = createCinemaRoomDto.SeatRows,
                SeatColumns = createCinemaRoomDto.SeatColumns
            };

            _context.CinemaRooms.Add(cinemaRoom);
            await _context.SaveChangesAsync();

            return new CinameRoomDto
            {
                Id = cinemaRoom.Id,
                Name = cinemaRoom.Name,
                CinemaName = cinema.Name,
                SeatRows = cinemaRoom.SeatRows,
                SeatColumns = cinemaRoom.SeatColumns
            };
        }

        public async Task<CinameRoomDto> UpdateCinemaRoomAsync(int id, UpdateCinemaRoomDto updateCinemaRoomDto)
        {
            var cinemaRoom = await _context.CinemaRooms.FindAsync(id);
            if (cinemaRoom == null) return null;

            cinemaRoom.Name = updateCinemaRoomDto.Name;
            cinemaRoom.SeatRows = updateCinemaRoomDto.SeatRows;
            cinemaRoom.SeatColumns = updateCinemaRoomDto.SeatColumns;

            await _context.SaveChangesAsync();

            return new CinameRoomDto
            {
                Id = cinemaRoom.Id,
                Name = cinemaRoom.Name,
                CinemaName = (await _context.Cinemas.FindAsync(cinemaRoom.CinemaId))?.Name,
                SeatRows = cinemaRoom.SeatRows,
                SeatColumns = cinemaRoom.SeatColumns
            };
        }

        public async Task<bool> DeleteCinemaRoomAsync(int id)
        {
            var cinemaRoom = await _context.CinemaRooms.FindAsync(id);
            if (cinemaRoom == null) return false;

            _context.CinemaRooms.Remove(cinemaRoom);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

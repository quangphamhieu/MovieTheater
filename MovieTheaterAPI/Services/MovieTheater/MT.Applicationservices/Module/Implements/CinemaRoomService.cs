using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MT.Applicationservices.Common;
using MT.Applicationservices.Module.Abstracts;
using MT.Domain;
using MT.Dtos.CinemaRoom;
using MT.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Applicationservices.Module.Implements
{
    public class CinemaRoomService : MovieTheaterBaseService, ICinemaRoomService
    {
        public CinemaRoomService(ILogger<CinemaRoomService> logger, MovieTheaterDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public async Task<IEnumerable<CinameRoomDto>> GetAllCinemaRoomsAsync()
        {
            _logger.LogInformation("Fetching all cinema rooms.");
            return await _dbContext.CinemaRooms
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
            _logger.LogInformation($"Fetching cinema room with ID {id}.");
            var cinemaRoom = await _dbContext.CinemaRooms
                .Include(cr => cr.Cinema)
                .FirstOrDefaultAsync(cr => cr.Id == id);

            if (cinemaRoom == null)
            {
                _logger.LogWarning($"Cinema room with ID {id} not found.");
                return null;
            }

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
            _logger.LogInformation($"Creating a new cinema room: {createCinemaRoomDto.Name}.");

            // Kiểm tra xem rạp có tồn tại không
            var cinema = await _dbContext.Cinemas.FirstOrDefaultAsync(c => c.Name == createCinemaRoomDto.CinemaName);
            if (cinema == null)
            {
                _logger.LogWarning($"Cinema with name {createCinemaRoomDto.CinemaName} not found.");
                throw new Exception("Cinema not found.");
            }

            var cinemaRoom = new CinemaRoom
            {
                Name = createCinemaRoomDto.Name,
                CinemaId = cinema.Id,
                SeatRows = createCinemaRoomDto.SeatRows,
                SeatColumns = createCinemaRoomDto.SeatColumns
            };

            _dbContext.CinemaRooms.Add(cinemaRoom);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Cinema room {cinemaRoom.Name} created successfully with ID {cinemaRoom.Id}.");
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
            _logger.LogInformation($"Updating cinema room with ID {id}.");
            var cinemaRoom = await _dbContext.CinemaRooms.FindAsync(id);

            if (cinemaRoom == null)
            {
                _logger.LogWarning($"Cinema room with ID {id} not found.");
                return null;
            }

            cinemaRoom.Name = updateCinemaRoomDto.Name;
            cinemaRoom.SeatRows = updateCinemaRoomDto.SeatRows;
            cinemaRoom.SeatColumns = updateCinemaRoomDto.SeatColumns;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Cinema room with ID {id} updated successfully.");
            return new CinameRoomDto
            {
                Id = cinemaRoom.Id,
                Name = cinemaRoom.Name,
                CinemaName = (await _dbContext.Cinemas.FindAsync(cinemaRoom.CinemaId))?.Name,
                SeatRows = cinemaRoom.SeatRows,
                SeatColumns = cinemaRoom.SeatColumns
            };
        }

        public async Task<bool> DeleteCinemaRoomAsync(int id)
        {
            _logger.LogInformation($"Deleting cinema room with ID {id}.");
            var cinemaRoom = await _dbContext.CinemaRooms.FindAsync(id);

            if (cinemaRoom == null)
            {
                _logger.LogWarning($"Cinema room with ID {id} not found.");
                return false;
            }

            _dbContext.CinemaRooms.Remove(cinemaRoom);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Cinema room with ID {id} deleted successfully.");
            return true;
        }
    }
}

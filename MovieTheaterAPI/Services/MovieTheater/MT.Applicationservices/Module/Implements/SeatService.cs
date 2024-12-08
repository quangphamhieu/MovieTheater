using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MT.Applicationservices.Common;
using MT.Applicationservices.Module.Abstracts;
using MT.Domain;
using MT.Dtos.Seat;
using MT.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Applicationservices.Module.Implements
{
    public class SeatService : MovieTheaterBaseService, ISeatService
    {
        public SeatService(ILogger<SeatService> logger, MovieTheaterDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public async Task<List<SeatDto>> GenerateSeatsAsync(int cinemaRoomId)
        {
            _logger.LogInformation($"Generating seats for Cinema Room with ID {cinemaRoomId}.");

            var room = await _dbContext.CinemaRooms.FindAsync(cinemaRoomId);
            if (room == null)
            {
                _logger.LogWarning($"Cinema Room with ID {cinemaRoomId} not found.");
                throw new Exception("Cinema Room not found");
            }

            var seats = new List<Seat>();
            for (int i = 0; i < room.SeatRows; i++)
            {
                for (int j = 1; j <= room.SeatColumns; j++)
                {
                    var seatCode = $"{(char)('A' + i)}{j}";
                    seats.Add(new Seat
                    {
                        CinemaRoomId = cinemaRoomId,
                        SeatCode = seatCode,
                        SeatType = "Regular",
                        Price = 60000 // giá mặc định
                    });
                }
            }

            await _dbContext.Seats.AddRangeAsync(seats);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Generated {seats.Count} seats for Cinema Room with ID {cinemaRoomId}.");

            return seats.Select(s => new SeatDto(s)).ToList();
        }

        public async Task<List<SeatDto>> GetSeatsByRoomAsync(int cinemaRoomId)
        {
            _logger.LogInformation($"Fetching seats for Cinema Room with ID {cinemaRoomId}.");

            var seats = await _dbContext.Seats
                .Where(s => s.CinemaRoomId == cinemaRoomId)
                .ToListAsync();

            _logger.LogInformation($"Found {seats.Count} seats for Cinema Room with ID {cinemaRoomId}.");

            return seats.Select(s => new SeatDto(s)).ToList();
        }
    }
}

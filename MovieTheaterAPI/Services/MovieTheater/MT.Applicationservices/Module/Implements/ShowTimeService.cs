using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MT.Applicationservices.Common;
using MT.Applicationservices.Module.Abstracts;
using MT.Domain;
using MT.Dtos.ShowTime;
using MT.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Applicationservices.Module.Implements
{
    public class ShowTimeService : MovieTheaterBaseService, IShowTimeService
    {
        public ShowTimeService(ILogger<ShowTimeService> logger, MovieTheaterDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public async Task<IEnumerable<ShowTimeDto>> GetAllShowTimesAsync()
        {
            _logger.LogInformation("Fetching all showtimes.");

            return await _dbContext.ShowTimes
                .Include(st => st.Movie)
                .Include(st => st.CinemaRoom)
                    .ThenInclude(cr => cr.Cinema) // Load Cinema through CinemaRoom
                .Select(st => new ShowTimeDto
                {
                    Id = st.Id,
                    MovieTitle = st.Movie.Title,
                    CinemaName = st.CinemaRoom.Cinema.Name, // Access Cinema via CinemaRoom
                    CinemaRoomName = st.CinemaRoom.Name,
                    StartTime = st.StartTime,
                    EndTime = st.EndTime
                })
            .ToListAsync();
        }

        public async Task<ShowTimeDto> GetShowTimeByIdAsync(int id)
        {
            _logger.LogInformation($"Fetching showtime with ID {id}.");

            var showTime = await _dbContext.ShowTimes
                .Include(st => st.Movie)
                .Include(st => st.CinemaRoom)
                    .ThenInclude(cr => cr.Cinema) // Load Cinema through CinemaRoom
                .FirstOrDefaultAsync(st => st.Id == id);

            if (showTime == null)
            {
                _logger.LogWarning($"Showtime with ID {id} not found.");
                return null;
            }

            return new ShowTimeDto
            {
                Id = showTime.Id,
                MovieTitle = showTime.Movie.Title,
                CinemaName = showTime.CinemaRoom.Cinema.Name, // Access Cinema via CinemaRoom
                CinemaRoomName = showTime.CinemaRoom.Name,
                StartTime = showTime.StartTime,
                EndTime = showTime.EndTime
            };
        }

        public async Task<ShowTimeDto> CreateShowTimeAsync(CreateShowTimeDto createShowTimeDto)
        {
            _logger.LogInformation($"Creating new showtime for movie '{createShowTimeDto.MovieTitle}' in room '{createShowTimeDto.CinemaRoomName}'.");

            var movie = await _dbContext.Movies.FirstOrDefaultAsync(m => m.Title == createShowTimeDto.MovieTitle);
            var cinemaRoom = await _dbContext.CinemaRooms
                .Include(cr => cr.Cinema) // Include Cinema for setting up DTO
                .FirstOrDefaultAsync(cr => cr.Name == createShowTimeDto.CinemaRoomName && cr.Cinema.Name == createShowTimeDto.CinemaName);

            if (movie == null || cinemaRoom == null)
            {
                _logger.LogWarning($"Invalid movie or cinema room for creating showtime.");
                return null;
            }

            var showTime = new ShowTime
            {
                MovieId = movie.Id,
                CinemaRoomId = cinemaRoom.Id,
                StartTime = createShowTimeDto.StartTime,
                EndTime = createShowTimeDto.EndTime
            };

            _dbContext.ShowTimes.Add(showTime);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Created new showtime with ID {showTime.Id}.");

            return new ShowTimeDto
            {
                Id = showTime.Id,
                MovieTitle = movie.Title,
                CinemaName = cinemaRoom.Cinema.Name,
                CinemaRoomName = cinemaRoom.Name,
                StartTime = showTime.StartTime,
                EndTime = showTime.EndTime
            };
        }

        public async Task<ShowTimeDto> UpdateShowTimeAsync(int id, UpdateShowTimeDto updateShowTimeDto)
        {
            _logger.LogInformation($"Updating showtime with ID {id}.");

            var showTime = await _dbContext.ShowTimes.FindAsync(id);
            if (showTime == null)
            {
                _logger.LogWarning($"Showtime with ID {id} not found.");
                return null;
            }

            var movie = await _dbContext.Movies.FirstOrDefaultAsync(m => m.Title == updateShowTimeDto.MovieTitle);
            var cinemaRoom = await _dbContext.CinemaRooms
                .Include(cr => cr.Cinema) // Include Cinema for updating DTO
                .FirstOrDefaultAsync(cr => cr.Name == updateShowTimeDto.CinemaRoomName && cr.Cinema.Name == updateShowTimeDto.CinemaName);

            if (movie == null || cinemaRoom == null)
            {
                _logger.LogWarning($"Invalid movie or cinema room for updating showtime.");
                return null;
            }

            showTime.MovieId = movie.Id;
            showTime.CinemaRoomId = cinemaRoom.Id;
            showTime.StartTime = updateShowTimeDto.StartTime;
            showTime.EndTime = updateShowTimeDto.EndTime;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Updated showtime with ID {id}.");

            return new ShowTimeDto
            {
                Id = showTime.Id,
                MovieTitle = movie.Title,
                CinemaName = cinemaRoom.Cinema.Name,
                CinemaRoomName = cinemaRoom.Name,
                StartTime = showTime.StartTime,
                EndTime = showTime.EndTime
            };
        }

        public async Task<bool> DeleteShowTimeAsync(int id)
        {
            _logger.LogInformation($"Deleting showtime with ID {id}.");

            var showTime = await _dbContext.ShowTimes.FindAsync(id);
            if (showTime == null)
            {
                _logger.LogWarning($"Showtime with ID {id} not found.");
                return false;
            }

            _dbContext.ShowTimes.Remove(showTime);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Deleted showtime with ID {id}.");
            return true;
        }
    }
}

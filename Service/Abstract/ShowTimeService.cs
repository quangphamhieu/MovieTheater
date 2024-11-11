using MovieTheater.DbContexts;
using MovieTheater.Dtos.ShowTime;
using MovieTheater.Entities;
using Microsoft.EntityFrameworkCore;
using MovieTheater.Service.Implement;

namespace MovieTheater.Service.Abstract
{
    public class ShowTimeService : IShowTimeService
    {
        private readonly ApplicationDbContext _context;

        public ShowTimeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ShowTimeDto>> GetAllShowTimesAsync()
        {
            return await _context.ShowTimes
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
            var showTime = await _context.ShowTimes
                .Include(st => st.Movie)
                .Include(st => st.CinemaRoom)
                    .ThenInclude(cr => cr.Cinema) // Load Cinema through CinemaRoom
                .FirstOrDefaultAsync(st => st.Id == id);

            if (showTime == null) return null;

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
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Title == createShowTimeDto.MovieTitle);
            var cinemaRoom = await _context.CinemaRooms
                .Include(cr => cr.Cinema) // Include Cinema for setting up DTO
                .FirstOrDefaultAsync(cr => cr.Name == createShowTimeDto.CinemaRoomName && cr.Cinema.Name == createShowTimeDto.CinemaName);

            if (movie == null || cinemaRoom == null) return null;

            var showTime = new ShowTime
            {
                MovieId = movie.Id,
                CinemaRoomId = cinemaRoom.Id,
                StartTime = createShowTimeDto.StartTime,
                EndTime = createShowTimeDto.EndTime
            };

            _context.ShowTimes.Add(showTime);
            await _context.SaveChangesAsync();

            return new ShowTimeDto
            {
                Id = showTime.Id,
                MovieTitle = movie.Title,
                CinemaName = cinemaRoom.Cinema.Name, // Access Cinema from CinemaRoom
                CinemaRoomName = cinemaRoom.Name,
                StartTime = showTime.StartTime,
                EndTime = showTime.EndTime
            };
        }

        public async Task<ShowTimeDto> UpdateShowTimeAsync(int id, UpdateShowTimeDto updateShowTimeDto)
        {
            var showTime = await _context.ShowTimes.FindAsync(id);
            if (showTime == null) return null;

            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Title == updateShowTimeDto.MovieTitle);
            var cinemaRoom = await _context.CinemaRooms
                .Include(cr => cr.Cinema) // Include Cinema for updating DTO
                .FirstOrDefaultAsync(cr => cr.Name == updateShowTimeDto.CinemaRoomName && cr.Cinema.Name == updateShowTimeDto.CinemaName);

            if (movie == null || cinemaRoom == null) return null;

            showTime.MovieId = movie.Id;
            showTime.CinemaRoomId = cinemaRoom.Id;
            showTime.StartTime = updateShowTimeDto.StartTime;
            showTime.EndTime = updateShowTimeDto.EndTime;

            await _context.SaveChangesAsync();

            return new ShowTimeDto
            {
                Id = showTime.Id,
                MovieTitle = movie.Title,
                CinemaName = cinemaRoom.Cinema.Name, // Access Cinema from CinemaRoom
                CinemaRoomName = cinemaRoom.Name,
                StartTime = showTime.StartTime,
                EndTime = showTime.EndTime
            };
        }

        public async Task<bool> DeleteShowTimeAsync(int id)
        {
            var showTime = await _context.ShowTimes.FindAsync(id);
            if (showTime == null) return false;

            _context.ShowTimes.Remove(showTime);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

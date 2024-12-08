using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MT.Applicationservices.Common;
using MT.Applicationservices.Module.Abstracts;
using MT.Domain;
using MT.Dtos.Cinema;
using MT.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Applicationservices.Module.Implements
{
    public class CinemaService : MovieTheaterBaseService, ICinemaService
    {
        public CinemaService(ILogger<CinemaService> logger, MovieTheaterDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public async Task<List<CinemaDto>> GetAllCinemasAsync()
        {
            _logger.LogInformation("Fetching all cinemas.");
            return await _dbContext.Cinemas
                .Select(c => new CinemaDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Location = c.Address
                })
            .ToListAsync();
        }

        public async Task<CinemaDto> GetCinemaByIdAsync(int id)
        {
            _logger.LogInformation($"Fetching cinema with ID {id}.");
            var cinema = await _dbContext.Cinemas.FindAsync(id);

            if (cinema == null)
            {
                _logger.LogWarning($"Cinema with ID {id} not found.");
                return null;
            }

            return new CinemaDto
            {
                Id = cinema.Id,
                Name = cinema.Name,
                Location = cinema.Address
            };
        }

        public async Task<CinemaDto> CreateCinemaAsync(CreateCinemaDto createCinemaDto)
        {
            _logger.LogInformation($"Creating a new cinema: {createCinemaDto.Name}.");

            var cinema = new Cinema
            {
                Name = createCinemaDto.Name,
                Address = createCinemaDto.Location
            };

            _dbContext.Cinemas.Add(cinema);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Cinema {cinema.Name} created successfully with ID {cinema.Id}.");
            return new CinemaDto
            {
                Id = cinema.Id,
                Name = cinema.Name,
                Location = cinema.Address
            };
        }

        public async Task<CinemaDto> UpdateCinemaAsync(int id, UpdateCinemaDto updateCinemaDto)
        {
            _logger.LogInformation($"Updating cinema with ID {id}.");
            var cinema = await _dbContext.Cinemas.FindAsync(id);

            if (cinema == null)
            {
                _logger.LogWarning($"Cinema with ID {id} not found.");
                return null;
            }

            cinema.Name = updateCinemaDto.Name;
            cinema.Address = updateCinemaDto.Location;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Cinema with ID {id} updated successfully.");
            return new CinemaDto
            {
                Id = cinema.Id,
                Name = cinema.Name,
                Location = cinema.Address
            };
        }

        public async Task<bool> DeleteCinemaAsync(int id)
        {
            _logger.LogInformation($"Deleting cinema with ID {id}.");
            var cinema = await _dbContext.Cinemas.FindAsync(id);

            if (cinema == null)
            {
                _logger.LogWarning($"Cinema with ID {id} not found.");
                return false;
            }

            _dbContext.Cinemas.Remove(cinema);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Cinema with ID {id} deleted successfully.");
            return true;
        }
    }
}

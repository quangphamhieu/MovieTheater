using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MT.Applicationservices.Common;
using MT.Applicationservices.Module.Abstracts;
using MT.Domain;
using MT.Dtos.Genre;
using MT.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Applicationservices.Module.Implements
{
    public class GenreService : MovieTheaterBaseService, IGenreService
    {
        public GenreService(ILogger<GenreService> logger, MovieTheaterDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public async Task<List<GenreDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all genres.");
            return await _dbContext.Genres
                .Select(g => new GenreDto { Id = g.Id, Name = g.Name })
            .ToListAsync();
        }

        public async Task<GenreDto> GetByIdAsync(int id)
        {
            _logger.LogInformation($"Fetching genre with ID {id}.");
            var genre = await _dbContext.Genres.FindAsync(id);
            if (genre == null)
            {
                _logger.LogWarning($"Genre with ID {id} not found.");
                return null;
            }

            return new GenreDto { Id = genre.Id, Name = genre.Name };
        }

        public async Task<GenreDto> GetByNameAsync(string name)
        {
            _logger.LogInformation($"Fetching genre with name {name}.");
            var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Name == name);
            if (genre == null)
            {
                _logger.LogWarning($"Genre with name {name} not found.");
                return null;
            }

            return new GenreDto { Id = genre.Id, Name = genre.Name };
        }

        public async Task<GenreDto> CreateAsync(CreateGenreDto dto)
        {
            _logger.LogInformation($"Creating new genre with name {dto.Name}.");
            var genre = new Genre { Name = dto.Name };
            _dbContext.Genres.Add(genre);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Genre created successfully with ID {genre.Id}.");
            return new GenreDto { Id = genre.Id, Name = genre.Name };
        }

        public async Task<GenreDto> UpdateAsync(int id, UpdateGenreDto dto)
        {
            _logger.LogInformation($"Updating genre with ID {id}.");
            var genre = await _dbContext.Genres.FindAsync(id);
            if (genre == null)
            {
                _logger.LogWarning($"Genre with ID {id} not found.");
                return null;
            }

            genre.Name = dto.Name;
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Genre with ID {id} updated successfully.");
            return new GenreDto { Id = genre.Id, Name = genre.Name };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation($"Deleting genre with ID {id}.");
            var genre = await _dbContext.Genres.FindAsync(id);
            if (genre == null)
            {
                _logger.LogWarning($"Genre with ID {id} not found.");
                return false;
            }

            _dbContext.Genres.Remove(genre);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Genre with ID {id} deleted successfully.");
            return true;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MT.Applicationservices.Common;
using MT.Applicationservices.Module.Abstracts;
using MT.Domain;
using MT.Dtos.Director;
using MT.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Applicationservices.Module.Implements
{
    public class DirectorService : MovieTheaterBaseService, IDirectorService
    {
        public DirectorService(ILogger<DirectorService> logger, MovieTheaterDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public async Task<List<DirectorDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all directors.");
            return await _dbContext.Directors
                .Select(d => new DirectorDto { Id = d.Id, Name = d.Name })
            .ToListAsync();
        }

        public async Task<DirectorDto> GetByIdAsync(int id)
        {
            _logger.LogInformation($"Fetching director with ID {id}.");
            var director = await _dbContext.Directors.FindAsync(id);
            if (director == null)
            {
                _logger.LogWarning($"Director with ID {id} not found.");
                return null;
            }

            return new DirectorDto { Id = director.Id, Name = director.Name };
        }

        public async Task<DirectorDto> GetByNameAsync(string name)
        {
            _logger.LogInformation($"Fetching director with name {name}.");
            var director = await _dbContext.Directors.FirstOrDefaultAsync(d => d.Name == name);
            if (director == null)
            {
                _logger.LogWarning($"Director with name {name} not found.");
                return null;
            }

            return new DirectorDto { Id = director.Id, Name = director.Name };
        }

        public async Task<DirectorDto> CreateAsync(CreateDirectorDto dto)
        {
            _logger.LogInformation($"Creating new director with name {dto.Name}.");
            var director = new Director { Name = dto.Name };
            _dbContext.Directors.Add(director);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Director created successfully with ID {director.Id}.");
            return new DirectorDto { Id = director.Id, Name = director.Name };
        }

        public async Task<DirectorDto> UpdateAsync(int id, UpdateDirectorDto dto)
        {
            _logger.LogInformation($"Updating director with ID {id}.");
            var director = await _dbContext.Directors.FindAsync(id);
            if (director == null)
            {
                _logger.LogWarning($"Director with ID {id} not found.");
                return null;
            }

            director.Name = dto.Name;
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Director with ID {id} updated successfully.");
            return new DirectorDto { Id = director.Id, Name = director.Name };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation($"Deleting director with ID {id}.");
            var director = await _dbContext.Directors.FindAsync(id);
            if (director == null)
            {
                _logger.LogWarning($"Director with ID {id} not found.");
                return false;
            }

            _dbContext.Directors.Remove(director);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Director with ID {id} deleted successfully.");
            return true;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MT.Applicationservices.Common;
using MT.Applicationservices.Module.Abstracts;
using MT.Domain;
using MT.Dtos.Actor;
using MT.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Applicationservices.Module.Implements
{
    public class ActorService : MovieTheaterBaseService, IActorService
    {
        public ActorService(ILogger<ActorService> logger, MovieTheaterDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public async Task<List<ActorDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all actors from the database.");
            return await _dbContext.Actors
                .Select(a => new ActorDto { Id = a.Id, Name = a.Name })
            .ToListAsync();
        }

        public async Task<ActorDto> GetByIdAsync(int id)
        {
            _logger.LogInformation($"Fetching actor with ID {id}.");
            var actor = await _dbContext.Actors.FindAsync(id);

            if (actor == null)
            {
                _logger.LogWarning($"Actor with ID {id} not found.");
                return null;
            }

            return new ActorDto { Id = actor.Id, Name = actor.Name };
        }

        public async Task<ActorDto> GetByNameAsync(string name)
        {
            _logger.LogInformation($"Fetching actor with name {name}.");
            var actor = await _dbContext.Actors.FirstOrDefaultAsync(a => a.Name == name);

            if (actor == null)
            {
                _logger.LogWarning($"Actor with name {name} not found.");
                return null;
            }

            return new ActorDto { Id = actor.Id, Name = actor.Name };
        }

        public async Task<ActorDto> CreateAsync(CreateActorDto dto)
        {
            _logger.LogInformation($"Creating a new actor with name {dto.Name}.");

            // Kiểm tra xem actor đã tồn tại chưa
            if (_dbContext.Actors.Any(a => a.Name == dto.Name))
            {
                _logger.LogWarning($"Actor with name {dto.Name} already exists.");
                throw new Exception("Actor name already exists.");
            }

            var actor = new Actor { Name = dto.Name };
            _dbContext.Actors.Add(actor);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Actor {actor.Name} created successfully with ID {actor.Id}.");
            return new ActorDto { Id = actor.Id, Name = actor.Name };
        }

        public async Task<ActorDto> UpdateAsync(int id, UpdateActorDto dto)
        {
            _logger.LogInformation($"Updating actor with ID {id}.");
            var actor = await _dbContext.Actors.FindAsync(id);

            if (actor == null)
            {
                _logger.LogWarning($"Actor with ID {id} not found.");
                return null;
            }

            actor.Name = dto.Name;
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Actor with ID {id} updated successfully.");
            return new ActorDto { Id = actor.Id, Name = actor.Name };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation($"Deleting actor with ID {id}.");
            var actor = await _dbContext.Actors.FindAsync(id);

            if (actor == null)
            {
                _logger.LogWarning($"Actor with ID {id} not found.");
                return false;
            }

            _dbContext.Actors.Remove(actor);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Actor with ID {id} deleted successfully.");
            return true;
        }
    }
}

using MovieTheater.DbContexts;
using MovieTheater.Dtos.Actor;
using MovieTheater.Entities;
using MovieTheater.Service.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MovieTheater.Service.Implement
{
    public class ActorService : IActorService
    {
        private readonly ApplicationDbContext _context;

        public ActorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ActorDto>> GetAllAsync()
        {
            return await _context.Actors
                .Select(a => new ActorDto { Id = a.Id, Name = a.Name })
                .ToListAsync();
        }

        public async Task<ActorDto> GetByIdAsync(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            return actor == null ? null : new ActorDto { Id = actor.Id, Name = actor.Name };
        }

        public async Task<ActorDto> GetByNameAsync(string name)
        {
            var actor = await _context.Actors.FirstOrDefaultAsync(a => a.Name == name);
            return actor == null ? null : new ActorDto { Id = actor.Id, Name = actor.Name };
        }

        public async Task<ActorDto> CreateAsync(CreateActorDto dto)
        {
            var actor = new Actor { Name = dto.Name };
            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();
            return new ActorDto { Id = actor.Id, Name = actor.Name };
        }

        public async Task<ActorDto> UpdateAsync(int id, UpdateActorDto dto)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null) return null;

            actor.Name = dto.Name;
            await _context.SaveChangesAsync();
            return new ActorDto { Id = actor.Id, Name = actor.Name };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null) return false;

            _context.Actors.Remove(actor);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}

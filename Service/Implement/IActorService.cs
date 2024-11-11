using MovieTheater.Dtos.Actor;

namespace MovieTheater.Service.Implement
{
    public interface IActorService
    {
        Task<List<ActorDto>> GetAllAsync();
        Task<ActorDto> GetByIdAsync(int id);
        Task<ActorDto> GetByNameAsync(string name);
        Task<ActorDto> CreateAsync(CreateActorDto dto);
        Task<ActorDto> UpdateAsync(int id, UpdateActorDto dto);
        Task<bool> DeleteAsync(int id);
    }

}

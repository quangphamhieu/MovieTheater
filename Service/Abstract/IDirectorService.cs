using MovieTheater.Dtos.Director;

namespace MovieTheater.Service.Abstract
{
    public interface IDirectorService
    {
        Task<List<DirectorDto>> GetAllAsync();
        Task<DirectorDto> GetByIdAsync(int id);
        Task<DirectorDto> CreateAsync(CreateDirectorDto dto);
        Task<DirectorDto> UpdateAsync(int id, UpdateDirectorDto dto);
        Task<bool> DeleteAsync(int id);
    }

}

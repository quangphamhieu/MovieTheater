using MovieTheater.Dtos.Genre;

namespace MovieTheater.Service.Abstract
{
    public interface IGenreService
    {
        Task<List<GenreDto>> GetAllAsync();
        Task<GenreDto> GetByIdAsync(int id);
        Task<GenreDto> GetByNameAsync(string name);
        Task<GenreDto> CreateAsync(CreateGenreDto dto);
        Task<GenreDto> UpdateAsync(int id, UpdateGenreDto dto);
        Task<bool> DeleteAsync(int id);
    }

}

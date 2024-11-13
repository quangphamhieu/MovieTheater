using MovieTheater.Dtos.Movie;

namespace MovieTheater.Service.Abstract
{
    public interface IMovieService
    {
        Task<List<MovieDto>> GetAllAsync();
        Task<MovieDto> GetByIdAsync(int id);
        Task<MovieDto> GetByTitleAsync(string title);
        Task<MovieDto> CreateAsync(CreateMovieDto dto);
        Task<List<MovieDto>> SearchMoviesByTitleAsync(string title)
        Task<MovieDto> UpdateAsync(int id, UpdateMovieDto dto);
        Task<bool> DeleteAsync(int id);
    }
}

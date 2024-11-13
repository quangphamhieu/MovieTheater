using System.Collections.Generic;
using System.Threading.Tasks;
using MovieTheater.Dtos.Cinema;

namespace MovieTheater.Service.Abstract
{
    public interface ICinemaService
    {
        Task<List<CinemaDto>> GetAllCinemasAsync();
        Task<CinemaDto> GetCinemaByIdAsync(int id);
        Task<CinemaDto> CreateCinemaAsync(CreateCinemaDto createCinemaDto);
        Task<CinemaDto> UpdateCinemaAsync(int id, UpdateCinema updateCinemaDto);
        Task<bool> DeleteCinemaAsync(int id);
    }
}

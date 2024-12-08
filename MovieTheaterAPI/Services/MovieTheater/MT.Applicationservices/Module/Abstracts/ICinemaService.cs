using MT.Dtos.Cinema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Applicationservices.Module.Abstracts
{
    public interface ICinemaService
    {
        Task<List<CinemaDto>> GetAllCinemasAsync();
        Task<CinemaDto> GetCinemaByIdAsync(int id);
        Task<CinemaDto> CreateCinemaAsync(CreateCinemaDto createCinemaDto);
        Task<CinemaDto> UpdateCinemaAsync(int id, UpdateCinemaDto updateCinemaDto);
        Task<bool> DeleteCinemaAsync(int id);
    }
}

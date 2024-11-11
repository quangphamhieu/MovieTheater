using MovieTheater.Dtos.ShowTime;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTheater.Service.Implement
{
    public interface IShowTimeService
    {
        Task<IEnumerable<ShowTimeDto>> GetAllShowTimesAsync();
        Task<ShowTimeDto> GetShowTimeByIdAsync(int id);
        Task<ShowTimeDto> CreateShowTimeAsync(CreateShowTimeDto createShowTimeDto);
        Task<ShowTimeDto> UpdateShowTimeAsync(int id, UpdateShowTimeDto updateShowTimeDto);
        Task<bool> DeleteShowTimeAsync(int id);
    }
}

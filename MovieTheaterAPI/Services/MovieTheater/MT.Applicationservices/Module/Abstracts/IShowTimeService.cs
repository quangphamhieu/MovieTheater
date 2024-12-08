using MT.Dtos.ShowTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Applicationservices.Module.Abstracts
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

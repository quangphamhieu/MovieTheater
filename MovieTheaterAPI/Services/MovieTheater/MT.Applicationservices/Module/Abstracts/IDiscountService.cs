using MT.Dtos.Discount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Applicationservices.Module.Abstracts
{
    public interface IDiscountService
    {
        Task<IEnumerable<DiscountDto>> GetAllDiscountsAsync();
        Task<DiscountDto> GetDiscountByIdAsync(int id);
        Task<DiscountDto> CreateDiscountAsync(CreateDiscountDto createDiscountDto);
        Task<DiscountDto> UpdateDiscountAsync(int id, UpdateDiscountDto updateDiscountDto);
        Task<bool> DeleteDiscountAsync(int id);
        Task<bool> ApplyDiscountAsync(int userId, string code);
    }
}

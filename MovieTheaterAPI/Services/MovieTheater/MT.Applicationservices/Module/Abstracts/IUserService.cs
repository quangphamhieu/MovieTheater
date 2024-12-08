using MT.Dtos.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MT.Applicationservices.Module.Abstracts
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto> GetByIdAsync(int id);
        Task<UserDto> GetByEmailAsync(string email);
        Task<bool> RegisterUserAsync(CreateUserDto dto);
        UserDto FindUserById();
        Task<UserDto> UpdateAsync(int id, UpdateUserDto dto);
        Task<bool> UpdatePasswordAsync(int userId, UpdatePasswordDto dto);
        Task<bool> DeleteAsync(int id);

        // Thêm các chức năng liên quan đến email
        Task<bool> SendEmailConfirmationAsync(int userId);
        Task<bool> ConfirmEmailAsync(string token);
        Task<bool> ResendEmailConfirmationAsync(int userId);
        Task<bool> SendResetPasswordEmailAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
    }
}

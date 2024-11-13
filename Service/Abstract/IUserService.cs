using MovieTheater.Dtos.User;

namespace MovieTheater.Service.Abstract
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto> GetByIdAsync(int id);
        Task<UserDto> GetByEmailAsync(string email);
        Task<UserDto> CreateAsync(CreateUserDto dto);
        public UserDto FindUserById();
        Task<UserDto> UpdateAsync(int id, UpdateUserDto dto);
        Task<bool> UpdatePasswordAsync(int userId, UpdatePasswordDto dto);
        Task<bool> DeleteAsync(int id);
    }
}

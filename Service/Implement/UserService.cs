using Microsoft.EntityFrameworkCore;
using MovieTheater.DbContexts;
using MovieTheater.Dtos.User;
using MovieTheater.Entities;
using MovieTheater.Service.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTheater.Service.Implement
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // Lấy tất cả users
        public async Task<List<UserDto>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Address = u.Address,
                    RoleName = u.Role.RoleName
                })
                .ToListAsync();
        }

        // Lấy user theo Id
        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                RoleName = user.Role.RoleName
            };
        }

        // Lấy user theo Email
        public async Task<UserDto> GetByEmailAsync(string email)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address= user.Address,
                RoleName = user.Role.RoleName
            };
        }

        // Tạo user mới (mặc định RoleId là 2 - Khách hàng)
        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Password = dto.Password,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                RoleId = 2 // Mặc định RoleId = 2 cho Khách hàng
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Truy vấn lại để lấy RoleName
            var createdUser = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            return new UserDto
            {
                Id = createdUser.Id,
                FullName = createdUser.FullName,
                Email = createdUser.Email,
                PhoneNumber = createdUser.PhoneNumber,
                Address = createdUser.Address,
                RoleName = createdUser.Role?.RoleName // Lấy RoleName từ bảng Role
            };
        }

        public UserDto FindUserById()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("Id");
            var userId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

            var user = _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    FullName = u.FullName,
                    PhoneNumber = u.PhoneNumber,
                    Address = u.Address,
                    RoleName = u.Role.RoleName
                })
                .FirstOrDefault();

            return user;
        }
        // Cập nhật user
        public async Task<UserDto> UpdateAsync(int id, UpdateUserDto dto)
        {
            // Tìm kiếm người dùng theo ID
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            // Cập nhật thông tin người dùng
            user.FullName = dto.FullName;
            user.PhoneNumber = dto.PhoneNumber;
            user.Email = dto.Email;
            user.Address = dto.Address;

            await _context.SaveChangesAsync();

            // Trả về UserDto với RoleName
            var roleName = (await _context.Roles.FindAsync(user.RoleId))?.RoleName;

            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                RoleName = roleName
            };
        }

        public async Task<bool> UpdatePasswordAsync(int userId, UpdatePasswordDto dto)
        {
            // Tìm người dùng theo ID
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            // Kiểm tra mật khẩu hiện tại
            if (user.Password != dto.CurrentPassword)
                return false; // Mật khẩu hiện tại không đúng

            // Kiểm tra mật khẩu mới và xác nhận mật khẩu trùng khớp
            if (dto.NewPassword != dto.ConfirmPassword)
                return false; // Mật khẩu xác nhận không khớp

            // Cập nhật mật khẩu mới
            user.Password = dto.NewPassword;
            await _context.SaveChangesAsync();

            return true;
        }


        // Xóa user
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

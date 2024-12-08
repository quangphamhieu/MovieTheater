using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MT.Applicationservices.Common;
using MT.Applicationservices.Module.Abstracts;
using MT.Domain;
using MT.Dtos.User;
using MT.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MT.Applicationservices.Module.Implements
{
    public class UserService : MovieTheaterBaseService, IUserService
    {
        private readonly EmailService _emailService;

        public UserService(ILogger<UserService> logger, MovieTheaterDbContext dbContext, IHttpContextAccessor httpContextAccessor, EmailService emailService)
            : base(logger, dbContext, httpContextAccessor)
        {
            _emailService = emailService;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            return await _dbContext.Users
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

        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            return user == null ? null : new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                RoleName = user.Role.RoleName
            };
        }

        public async Task<UserDto> GetByEmailAsync(string email)
        {
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);

            return user == null ? null : new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                RoleName = user.Role.RoleName
            };
        }

        public async Task<bool> RegisterUserAsync(CreateUserDto dto)
        {
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (existingUser != null) return false;

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Password = dto.Password,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                RoleId = 2
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            // Generate and send email confirmation token
            var token = Guid.NewGuid().ToString();
            user.EmailConfirmToken = token;
            await _dbContext.SaveChangesAsync();

            // Tạo thông điệp email chi tiết hơn
            var emailContent = $@"
        Xin chào {user.FullName},

        Cảm ơn bạn đã đăng ký tài khoản tại hệ thống của chúng tôi.

        Dưới đây là thông tin tài khoản của bạn:
        - Email: {user.Email}
        - Mật khẩu: {user.Password}

        Mã xác thực của bạn là: {token}

        Vui lòng xác nhận email của bạn để hoàn tất quá trình đăng ký.

        Trân trọng,
        Đội ngũ hỗ trợ hệ thống.
    ";

            // Gửi email với thông điệp chi tiết
            await _emailService.SendEmailAsync(user.Email, "Xác nhận Email", emailContent);
            return true;
        }


        public UserDto FindUserById()
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext?.User.FindFirst("Id")?.Value ?? "0");
            var user = _dbContext.Users
                .Where(u => u.Id == userId)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Address = u.Address,
                    RoleName = u.Role.RoleName
                })
                .FirstOrDefault();

            return user;
        }

        public async Task<UserDto> UpdateAsync(int id, UpdateUserDto dto)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null) return null;

            user.FullName = dto.FullName;
            user.PhoneNumber = dto.PhoneNumber;
            user.Email = dto.Email;
            user.Address = dto.Address;
            await _dbContext.SaveChangesAsync();

            var roleName = (await _dbContext.Roles.FindAsync(user.RoleId))?.RoleName;
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
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null || user.Password != dto.CurrentPassword) return false;

            if (dto.NewPassword != dto.ConfirmPassword) return false;

            user.Password = dto.NewPassword;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null) return false;

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        // Email-related functions
        public async Task<bool> SendEmailConfirmationAsync(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null) return false;

            var token = Guid.NewGuid().ToString();
            user.EmailConfirmToken = token;
            await _dbContext.SaveChangesAsync();

            await _emailService.SendEmailAsync(user.Email, "Xác nhận Email", $"Mã xác thực của bạn: {token}");
            return true;
        }

        public async Task<bool> ConfirmEmailAsync(string token)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.EmailConfirmToken == token);
            if (user == null) return false;

            user.EmailConfirmToken = null;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ResendEmailConfirmationAsync(int userId)
        {
            return await SendEmailConfirmationAsync(userId);
        }

        public async Task<bool> SendResetPasswordEmailAsync(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return false;

            var token = Guid.NewGuid().ToString();
            user.EmailConfirmToken = token;
            await _dbContext.SaveChangesAsync();

            await _emailService.SendEmailAsync(email, "Đặt lại mật khẩu", $"Mã xác thực của bạn: {token}");
            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == dto.Email && u.EmailConfirmToken == dto.Token);
            if (user == null) return false;

            user.Password = dto.Password;
            user.EmailConfirmToken = null;
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}

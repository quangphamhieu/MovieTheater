using Microsoft.EntityFrameworkCore;
using MovieTheater.DbContexts;
using MovieTheater.Dtos.Role;
using MovieTheater.Entities;
using MovieTheater.Service.Abstract;

namespace MovieTheater.Service.Implement
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _context;

        public RoleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoleDto>> GetAllAsync()
        {
            return await _context.Roles
                .Select(r => new RoleDto { Id = r.Id, RoleName = r.RoleName })
                .ToListAsync();
        }

        public async Task<RoleDto> CreateAsync(CreateRoleDto dto)
        {
            var role = new Role { RoleName = dto.RoleName };
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return new RoleDto { Id = role.Id, RoleName = role.RoleName };
        }

        public async Task<RoleDto> UpdateAsync(int id, UpdateRoleDto dto)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return null;

            role.RoleName = dto.RoleName;
            await _context.SaveChangesAsync();
            return new RoleDto { Id = role.Id, RoleName = role.RoleName };
        }

    }
}

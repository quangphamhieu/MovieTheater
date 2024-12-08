using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MT.Applicationservices.Common;
using MT.Applicationservices.Module.Abstracts;
using MT.Domain;
using MT.Dtos.Role;
using MT.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Applicationservices.Module.Implements
{
    public class RoleService : MovieTheaterBaseService, IRoleService
    {
        public RoleService(ILogger<RoleService> logger, MovieTheaterDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public async Task<List<RoleDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all roles.");
            return await _dbContext.Roles
                .Select(r => new RoleDto { Id = r.Id, RoleName = r.RoleName })
                .ToListAsync();
        }

        public async Task<RoleDto> CreateAsync(CreateRoleDto dto)
        {
            _logger.LogInformation($"Creating a new role: {dto.RoleName}.");
            var role = new Role { RoleName = dto.RoleName };
            _dbContext.Roles.Add(role);
            await _dbContext.SaveChangesAsync();

            return new RoleDto { Id = role.Id, RoleName = role.RoleName };
        }

        public async Task<RoleDto> UpdateAsync(int id, UpdateRoleDto dto)
        {
            _logger.LogInformation($"Updating role with ID {id}.");
            var role = await _dbContext.Roles.FindAsync(id);
            if (role == null)
            {
                _logger.LogWarning($"Role with ID {id} not found.");
                return null;
            }

            role.RoleName = dto.RoleName;
            await _dbContext.SaveChangesAsync();

            return new RoleDto { Id = role.Id, RoleName = role.RoleName };
        }
    }
}

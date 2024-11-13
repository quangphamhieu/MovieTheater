using MovieTheater.Dtos.Role;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTheater.Service.Abstract
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetAllAsync();
        Task<RoleDto> CreateAsync(CreateRoleDto dto);
        Task<RoleDto> UpdateAsync(int id, UpdateRoleDto dto);

    }
}

using PhotographyPortfolioAPI.Contracts.Models.Auth;
using PhotographyPortfolioAPI.Infrastructure.Interfaces.Auth;
using PhotographyPortfolioAPI.Interfaces.Auth;

namespace PhotographyPortfolioAPI.Services.Auth
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepo;

        public RoleService(IRoleRepository roleRepo)
        {
            _roleRepo = roleRepo;
        }

        public async Task<Guid> CreateRoleAsync(RoleCreateRequest request)
        {
            var roleId = Guid.NewGuid();
            await _roleRepo.CreateAsync(new Role { role_id = roleId, role_code = request.role_code, role_name = request.role_name });
            return roleId;
        }

        public async Task<Guid> UpdateRoleAsync(Guid roleId, RoleUpdateRequest request)
        {
            var role = await _roleRepo.GetByIdAsync(roleId);
            if (role == null) throw new KeyNotFoundException("Role not found");
            role.role_name = request.role_name;
            await _roleRepo.UpdateAsync(role);
            return roleId;
        }

        public async Task<Role> GetRoleByIdAsync(Guid roleId)
        {
            var role = await _roleRepo.GetByIdAsync(roleId);
            if (role == null) throw new KeyNotFoundException("Role not found");
            return role;
        }

        public async Task<List<Role>> GetAllRolesAsync()
            => await _roleRepo.GetAllAsync();

        public async Task UpdateRolePermissionsAsync(Guid roleId, UpdateRolePermissionsRequestData permissionIds)
        {
            await _roleRepo.RemoveAllPermissionsAsync(roleId);
            foreach (var p in permissionIds.PermissionsList)
            {
                p.role_id = roleId;
                await _roleRepo.AddPermissionAsync(p);
            }
        }

        public async Task<List<Permission>> GetRolePermissionsAsync(Guid roleId)
            => await _roleRepo.GetRolePermissionsAsync(roleId);
    }
}

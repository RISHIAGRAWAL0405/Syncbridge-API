using PhotographyPortfolioAPI.Contracts.Models.Auth;

namespace PhotographyPortfolioAPI.Interfaces.Auth
{
    public interface IRoleService
    {
        Task<Guid> CreateRoleAsync(RoleCreateRequest request);
        Task<Guid> UpdateRoleAsync(Guid roleId, RoleUpdateRequest request);
        Task<Role> GetRoleByIdAsync(Guid roleId);
        Task<List<Role>> GetAllRolesAsync();
        Task UpdateRolePermissionsAsync(Guid roleId, UpdateRolePermissionsRequestData permissionIds);
        Task<List<Permission>> GetRolePermissionsAsync(Guid roleId);
    }
}

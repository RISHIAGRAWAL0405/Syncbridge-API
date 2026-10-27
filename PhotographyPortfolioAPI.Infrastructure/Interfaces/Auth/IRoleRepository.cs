using PhotographyPortfolioAPI.Contracts.Models.Auth;

namespace PhotographyPortfolioAPI.Infrastructure.Interfaces.Auth
{
    public interface IRoleRepository
    {
        Task CreateAsync(Role role);
        Task UpdateAsync(Role role);
        Task<Role?> GetByIdAsync(Guid roleId);
        Task<List<Role>> GetAllAsync();
        Task<Role?> GetByNameAsync(string roleName);
        Task RemoveAllPermissionsAsync(Guid roleId);
        Task AddPermissionAsync(UpdateRolePermissionsDetails updateRolePermissionsDetails);
        Task<List<Permission>> GetRolePermissionsAsync(Guid roleId);
    }
}

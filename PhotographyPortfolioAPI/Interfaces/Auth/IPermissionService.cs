using PhotographyPortfolioAPI.Contracts.Models.Auth;

namespace PhotographyPortfolioAPI.Interfaces.Auth
{
    public interface IPermissionService
    {
        Task<Guid> CreatePermissionAsync(PermissionCreateRequest request);
        Task<List<Permission>> GetAllPermissionsAsync();
    }
}

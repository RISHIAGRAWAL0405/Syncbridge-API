using PhotographyPortfolioAPI.Contracts.Models.Auth;

namespace PhotographyPortfolioAPI.Infrastructure.Interfaces.Auth
{
    public interface IPermissionRepository
    {
        Task CreateAsync(Permission permission);
        Task<List<Permission>> GetAllAsync();
    }
}

using PhotographyPortfolioAPI.Contracts.Models.Auth;

namespace PhotographyPortfolioAPI.Infrastructure.Interfaces.Auth
{
    public interface IJwtService
    {
        string GenerateAccessTokenAsync(User user, List<UserPermissionsData> permissions);
        string GenerateRefreshTokenAsync();
    }
}

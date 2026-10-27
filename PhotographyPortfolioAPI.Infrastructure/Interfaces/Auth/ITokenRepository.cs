using PhotographyPortfolioAPI.Contracts.Models.Auth;

namespace PhotographyPortfolioAPI.Infrastructure.Interfaces.Auth
{
    public interface ITokenRepository
    {
        Task SaveRefreshTokenAsync(RefreshToken token);
        Task<RefreshToken?> GetRefreshTokenAsync(string token);
        Task Revoke(string token);
    }
}

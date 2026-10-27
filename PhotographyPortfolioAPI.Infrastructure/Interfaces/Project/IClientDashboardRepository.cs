using PhotographyPortfolioAPI.Contracts.DTOs.Project;

namespace PhotographyPortfolioAPI.Infrastructure.Interfaces.Project
{
    public interface IClientDashboardRepository
    {
        Task<ClientDashboardDto> GetDashboardAsync(Guid clientId);
    }
}

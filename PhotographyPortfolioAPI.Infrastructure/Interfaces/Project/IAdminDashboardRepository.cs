using PhotographyPortfolioAPI.Contracts.DTOs.Project;

namespace PhotographyPortfolioAPI.Infrastructure.Interfaces.Project
{
    public interface IAdminDashboardRepository
    {
        Task<AdminDashboardDto> GetDashboardAsync();
    }
}

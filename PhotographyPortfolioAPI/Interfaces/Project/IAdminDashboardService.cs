using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;

namespace PhotographyPortfolioAPI.Interfaces.Project
{
    public interface IAdminDashboardService
    {
        Task<ApiResponseData<AdminDashboardDto>> GetDashboardAsync();
    }
}

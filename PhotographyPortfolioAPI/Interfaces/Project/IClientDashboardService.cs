using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;

namespace PhotographyPortfolioAPI.Interfaces.Project
{
    public interface IClientDashboardService
    {
        Task<ApiResponseData<ClientDashboardDto>> GetDashboardAsync(Guid clientId);
    }
}

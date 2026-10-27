using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Infrastructure.Interfaces.Project;
using PhotographyPortfolioAPI.Interfaces.Project;

namespace PhotographyPortfolioAPI.Services.Project
{
    public class ClientDashboardService : IClientDashboardService
    {
        private readonly IClientDashboardRepository _repo;

        public ClientDashboardService(IClientDashboardRepository repo) => _repo = repo;

        public async Task<ApiResponseData<ClientDashboardDto>> GetDashboardAsync(Guid clientId)
        {
            var dto = await _repo.GetDashboardAsync(clientId);
            return ApiResponseData<ClientDashboardDto>.Success(dto, "Dashboard loaded");
        }
    }
}

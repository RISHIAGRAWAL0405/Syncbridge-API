using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Infrastructure.Interfaces.Project;
using PhotographyPortfolioAPI.Interfaces.Project;

namespace PhotographyPortfolioAPI.Services.Project
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IAdminDashboardRepository _repo;

        public AdminDashboardService(IAdminDashboardRepository repo) => _repo = repo;

        public async Task<ApiResponseData<AdminDashboardDto>> GetDashboardAsync()
        {
            var dto = await _repo.GetDashboardAsync();
            return ApiResponseData<AdminDashboardDto>.Success(dto, "Admin dashboard loaded");
        }
    }
}

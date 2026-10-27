using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Interfaces.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PhotographyPortfolioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IAdminDashboardService _service;

        public AdminDashboardController(IAdminDashboardService service) => _service = service;

        [HttpGet]
        public async Task<ApiResponseData<AdminDashboardDto>> Get()
        {
            try { return await _service.GetDashboardAsync(); }
            catch (Exception) { return ApiResponseData<AdminDashboardDto>.Error("Failed to load admin dashboard", 500); }
        }
    }
}

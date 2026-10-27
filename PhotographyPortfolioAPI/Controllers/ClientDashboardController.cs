using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Extensions;
using PhotographyPortfolioAPI.Interfaces.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PhotographyPortfolioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientDashboardController : ControllerBase
    {
        private readonly IClientDashboardService _service;

        public ClientDashboardController(IClientDashboardService service) => _service = service;

        [HttpGet]
        public async Task<ApiResponseData<ClientDashboardDto>> Get()
        {
            try { return await _service.GetDashboardAsync(User.GetUserId()); }
            catch (Exception ex) { return ApiResponseData<ClientDashboardDto>.Error("Failed to load dashboard: " + ex.Message, 500); }
        }
    }
}

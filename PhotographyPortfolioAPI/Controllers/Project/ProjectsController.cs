using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Interfaces.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PhotographyPortfolioAPI.Controllers.Project
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _service;

        public ProjectsController(IProjectService service) => _service = service;

        [HttpPost]
        public async Task<ApiResponseData<object>> Create([FromBody] ProjectCreateRequest request)
        {
            try { return await _service.CreateAsync(request); }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred while creating project", 500); }
        }

        [HttpPut("{project_id}")]
        public async Task<ApiResponseData<object>> Update(Guid project_id, [FromBody] ProjectUpdateRequest request)
        {
            try { return await _service.UpdateAsync(project_id, request); }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred while updating project", 500); }
        }

        [HttpGet("{project_id}")]
        public async Task<ApiResponseData<ProjectDto>> GetById(Guid project_id)
        {
            try { return await _service.GetByIdAsync(project_id); }
            catch (KeyNotFoundException ex) { return ApiResponseData<ProjectDto>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<ProjectDto>.Error("An error occurred", 500); }
        }

        [HttpGet]
        public async Task<ApiResponseData<List<ProjectDto>>> GetAll()
        {
            try { return await _service.GetAllAsync(); }
            catch (Exception ex) { return ApiResponseData<List<ProjectDto>>.Error("An error occurred", 500); }
        }

        [HttpGet("by-client/{client_id}")]
        public async Task<ApiResponseData<List<ProjectDto>>> GetByClient(Guid client_id)
        {
            try { return await _service.GetByClientIdAsync(client_id); }
            catch (Exception) { return ApiResponseData<List<ProjectDto>>.Error("An error occurred", 500); }
        }

        [HttpPatch("{project_id}/status")]
        public async Task<ApiResponseData<object>> ActiveInactive(Guid project_id, [FromQuery] bool is_active)
        {
            try { return await _service.ActiveInactiveAsync(project_id, is_active); }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred", 500); }
        }

        [HttpPatch("{project_id}/project-status")]
        public async Task<ApiResponseData<object>> UpdateStatus(Guid project_id, [FromBody] ProjectStatusUpdateRequest request)
        {
            try { return await _service.UpdateStatusAsync(project_id, request); }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred while updating project status", 500); }
        }

        [HttpGet("{project_id}/profile")]
        public async Task<ApiResponseData<ProjectProfileDto>> GetProfile(Guid project_id)
        {
            try { return await _service.GetProjectProfileAsync(project_id); }
            catch (KeyNotFoundException ex) { return ApiResponseData<ProjectProfileDto>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<ProjectProfileDto>.Error("An error occurred", 500); }
        }
    }
}

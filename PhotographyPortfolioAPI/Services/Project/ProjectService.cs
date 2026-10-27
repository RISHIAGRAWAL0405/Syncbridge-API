using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Contracts.Models.Project;
using PhotographyPortfolioAPI.Core.Enums.Project;
using PhotographyPortfolioAPI.Infrastructure.Interfaces.Project;
using PhotographyPortfolioAPI.Interfaces.Project;

namespace PhotographyPortfolioAPI.Services.Project
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _repo;
        private readonly IClientRepository _clientRepo;

        public ProjectService(IProjectRepository repo, IClientRepository clientRepo)
        {
            _repo = repo;
            _clientRepo = clientRepo;
        }

        public async Task<ApiResponseData<object>> CreateAsync(ProjectCreateRequest request)
        {
            var client = await _clientRepo.GetByIdAsync(request.client_id);
            if (client == null) throw new KeyNotFoundException("Client not found");

            var project = new ProjectMaster
            {
                project_id = Guid.NewGuid(),
                client_id = request.client_id,
                project_name = request.project_name,
                project_code = request.project_code,
                description = request.description,
                start_date = request.start_date,
                end_date = request.end_date,
                status = (request.status ?? ProjectStatus.NEW).ToString(),
                is_active = true,
                created_at = DateTime.UtcNow
            };
            var id = await _repo.CreateAsync(project);
            return ApiResponseData<object>.Success(null, "Project created successfully", id);
        }

        public async Task<ApiResponseData<object>> UpdateAsync(Guid projectId, ProjectUpdateRequest request)
        {
            var project = await _repo.GetByIdAsync(projectId);
            if (project == null) throw new KeyNotFoundException("Project not found");

            if (!string.IsNullOrWhiteSpace(request.project_name)) project.project_name = request.project_name;
            if (!string.IsNullOrWhiteSpace(request.project_code)) project.project_code = request.project_code;
            if (!string.IsNullOrWhiteSpace(request.description)) project.description = request.description;
            if (request.start_date.HasValue) project.start_date = request.start_date;
            if (request.end_date.HasValue) project.end_date = request.end_date;
            if (request.status.HasValue) project.status = request.status.Value.ToString();
            project.updated_at = DateTime.UtcNow;

            await _repo.UpdateAsync(project);
            return ApiResponseData<object>.Success(null, "Project updated successfully", projectId);
        }

        public async Task<ApiResponseData<ProjectDto>> GetByIdAsync(Guid projectId)
        {
            var project = await _repo.GetByIdAsync(projectId);
            if (project == null) throw new KeyNotFoundException("Project not found");
            var dto = new ProjectDto
            {
                project_id = project.project_id,
                client_id = project.client_id,
                project_name = project.project_name,
                project_code = project.project_code,
                description = project.description,
                start_date = project.start_date,
                end_date = project.end_date,
                status = project.status,
                is_active = project.is_active,
                created_at = project.created_at
            };
            return ApiResponseData<ProjectDto>.Success(dto, "Project retrieved successfully");
        }

        public async Task<ApiResponseData<List<ProjectDto>>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return ApiResponseData<List<ProjectDto>>.Success(list, "Projects retrieved successfully");
        }

        public async Task<ApiResponseData<List<ProjectDto>>> GetByClientIdAsync(Guid clientId)
        {
            var list = await _repo.GetByClientIdAsync(clientId);
            return ApiResponseData<List<ProjectDto>>.Success(list, "Projects retrieved successfully");
        }

        public async Task<ApiResponseData<object>> ActiveInactiveAsync(Guid projectId, bool isActive)
        {
            var project = await _repo.GetByIdAsync(projectId);
            if (project == null) throw new KeyNotFoundException("Project not found");
            await _repo.ActiveInactiveAsync(projectId, isActive);
            return ApiResponseData<object>.Success(new { id = projectId }, "Project status updated successfully");
        }

        public async Task<ApiResponseData<object>> UpdateStatusAsync(Guid projectId, ProjectStatusUpdateRequest request)
        {
            var project = await _repo.GetByIdAsync(projectId);
            if (project == null) throw new KeyNotFoundException("Project not found");
            await _repo.UpdateStatusAsync(projectId, request.status.ToString());
            return ApiResponseData<object>.Success(new { id = projectId, status = request.status.ToString() }, "Project status updated successfully");
        }

        public async Task<ApiResponseData<ProjectProfileDto>> GetProjectProfileAsync(Guid projectId)
        {
            var profile = await _repo.GetProjectProfileAsync(projectId);
            if (profile == null) throw new KeyNotFoundException("Project not found");
            return ApiResponseData<ProjectProfileDto>.Success(profile, "Project profile retrieved successfully");
        }
    }
}

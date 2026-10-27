using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;

namespace PhotographyPortfolioAPI.Interfaces.Project
{
    public interface IProjectService
    {
        Task<ApiResponseData<object>> CreateAsync(ProjectCreateRequest request);
        Task<ApiResponseData<object>> UpdateAsync(Guid projectId, ProjectUpdateRequest request);
        Task<ApiResponseData<ProjectDto>> GetByIdAsync(Guid projectId);
        Task<ApiResponseData<List<ProjectDto>>> GetAllAsync();
        Task<ApiResponseData<List<ProjectDto>>> GetByClientIdAsync(Guid clientId);
        Task<ApiResponseData<object>> ActiveInactiveAsync(Guid projectId, bool isActive);
        Task<ApiResponseData<object>> UpdateStatusAsync(Guid projectId, ProjectStatusUpdateRequest request);
        Task<ApiResponseData<ProjectProfileDto>> GetProjectProfileAsync(Guid projectId);
    }
}

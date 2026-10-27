using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Contracts.Models.Project;

namespace PhotographyPortfolioAPI.Infrastructure.Interfaces.Project
{
    public interface IProjectRepository
    {
        Task<Guid> CreateAsync(ProjectMaster project);
        Task UpdateAsync(ProjectMaster project);
        Task<ProjectMaster?> GetByIdAsync(Guid projectId);
        Task<List<ProjectDto>> GetAllAsync();
        Task<List<ProjectDto>> GetByClientIdAsync(Guid clientId);
        Task ActiveInactiveAsync(Guid projectId, bool isActive);
        Task UpdateStatusAsync(Guid projectId, string status);
        Task<ProjectProfileDto> GetProjectProfileAsync(Guid projectId);
    }
}

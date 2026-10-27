using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Contracts.Models.Project;

namespace PhotographyPortfolioAPI.Infrastructure.Interfaces.Project
{
    public interface IStepLinkRepository
    {
        Task<Guid> CreateAsync(TaskWorkflowStepLink link);
        Task UpdateAsync(TaskWorkflowStepLink link);
        Task<TaskWorkflowStepLink?> GetByIdAsync(Guid linkId);
        Task<List<StepLinkDto>> GetByStepIdAsync(Guid taskWorkflowStepId);
        Task<List<StepLinkDto>> GetByTaskIdAsync(Guid taskId);
        Task ReviewAsync(Guid linkId, string status, string? statusNote, Guid reviewedBy);
        Task DeleteAsync(Guid linkId);
    }
}

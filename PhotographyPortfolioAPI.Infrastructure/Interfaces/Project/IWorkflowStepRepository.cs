using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Contracts.Models.Project;

namespace PhotographyPortfolioAPI.Infrastructure.Interfaces.Project
{
    public interface IWorkflowStepRepository
    {
        Task<Guid> CreateAsync(WorkflowStepMaster step);
        Task UpdateAsync(WorkflowStepMaster step);
        Task<WorkflowStepMaster?> GetByIdAsync(Guid stepId);
        Task<List<WorkflowStepDto>> GetAllAsync();
        Task<List<WorkflowStepDto>> GetAllActiveAsync();
        Task ActiveInactiveAsync(Guid stepId, bool isActive);
    }
}

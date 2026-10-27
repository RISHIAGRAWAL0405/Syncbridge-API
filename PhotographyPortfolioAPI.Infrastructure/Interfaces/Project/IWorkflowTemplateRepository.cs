using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Contracts.Models.Project;

namespace PhotographyPortfolioAPI.Infrastructure.Interfaces.Project
{
    public interface IWorkflowTemplateRepository
    {
        Task<Guid> CreateAsync(WorkflowTemplate template);
        Task UpdateAsync(WorkflowTemplate template);
        Task<WorkflowTemplate?> GetByIdAsync(Guid templateId);
        Task<List<WorkflowTemplateDto>> GetAllAsync();
        Task<WorkflowTemplateDto?> GetWithStepsByIdAsync(Guid templateId);
        Task ActiveInactiveAsync(Guid templateId, bool isActive);

        Task AddStepAsync(WorkflowTemplateStep step);
        Task RemoveAllStepsAsync(Guid templateId);
        Task<List<WorkflowTemplateStepDto>> GetStepsByTemplateIdAsync(Guid templateId);
    }
}

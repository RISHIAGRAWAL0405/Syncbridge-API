using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;

namespace PhotographyPortfolioAPI.Interfaces.Project
{
    public interface IWorkflowTemplateService
    {
        Task<ApiResponseData<object>> CreateAsync(WorkflowTemplateCreateRequest request);
        Task<ApiResponseData<object>> UpdateAsync(Guid templateId, WorkflowTemplateUpdateRequest request);
        Task<ApiResponseData<WorkflowTemplateDto>> GetByIdAsync(Guid templateId);
        Task<ApiResponseData<List<WorkflowTemplateDto>>> GetAllAsync();
        Task<ApiResponseData<object>> ActiveInactiveAsync(Guid templateId, bool isActive);
        Task<ApiResponseData<object>> UpdateStepsAsync(Guid templateId, List<WorkflowTemplateStepRequest> steps);
        Task<ApiResponseData<List<WorkflowTemplateStepDto>>> GetStepsAsync(Guid templateId);
    }
}

using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;

namespace PhotographyPortfolioAPI.Interfaces.Project
{
    public interface IWorkflowStepService
    {
        Task<ApiResponseData<object>> CreateAsync(WorkflowStepCreateRequest request);
        Task<ApiResponseData<object>> UpdateAsync(Guid stepId, WorkflowStepUpdateRequest request);
        Task<ApiResponseData<WorkflowStepDto>> GetByIdAsync(Guid stepId);
        Task<ApiResponseData<List<WorkflowStepDto>>> GetAllAsync();
        Task<ApiResponseData<List<WorkflowStepDto>>> GetAllActiveAsync();
        Task<ApiResponseData<object>> ActiveInactiveAsync(Guid stepId, bool isActive);
    }
}

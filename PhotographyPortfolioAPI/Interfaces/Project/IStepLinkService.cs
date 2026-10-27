using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;

namespace PhotographyPortfolioAPI.Interfaces.Project
{
    public interface IStepLinkService
    {
        Task<ApiResponseData<object>> CreateAsync(Guid taskWorkflowStepId, StepLinkCreateRequest request, Guid userId);
        Task<ApiResponseData<object>> UpdateAsync(Guid linkId, StepLinkUpdateRequest request);
        Task<ApiResponseData<List<StepLinkDto>>> GetByStepIdAsync(Guid taskWorkflowStepId);
        Task<ApiResponseData<List<StepLinkDto>>> GetByTaskIdAsync(Guid taskId);
        Task<ApiResponseData<object>> ReviewAsync(Guid linkId, StepLinkReviewRequest request, Guid reviewedBy);
        Task<ApiResponseData<object>> DeleteAsync(Guid linkId);
    }
}

using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Contracts.Models.Project;
using PhotographyPortfolioAPI.Infrastructure.Interfaces.Project;
using PhotographyPortfolioAPI.Interfaces.Project;

namespace PhotographyPortfolioAPI.Services.Project
{
    public class StepLinkService : IStepLinkService
    {
        private readonly IStepLinkRepository _repo;
        private readonly ITaskRepository _taskRepo;

        public StepLinkService(IStepLinkRepository repo, ITaskRepository taskRepo)
        {
            _repo = repo;
            _taskRepo = taskRepo;
        }

        public async Task<ApiResponseData<object>> CreateAsync(Guid taskWorkflowStepId, StepLinkCreateRequest request, Guid userId)
        {
            var step = await _taskRepo.GetWorkflowStepByIdAsync(taskWorkflowStepId)
                ?? throw new KeyNotFoundException("Workflow step not found");

            var link = new TaskWorkflowStepLink
            {
                link_id = Guid.NewGuid(),
                task_workflow_step_id = taskWorkflowStepId,
                task_id = step.task_id,
                category = request.category,
                title = request.title,
                url = request.url,
                link_type = (request.link_type ?? "VIDEO").ToUpper(),
                description = request.description,
                status = "PENDING",
                uploaded_by = userId,
                is_active = true,
                created_at = DateTime.UtcNow
            };

            var id = await _repo.CreateAsync(link);
            return ApiResponseData<object>.Success(null, "Link added successfully", id: id);
        }

        public async Task<ApiResponseData<object>> UpdateAsync(Guid linkId, StepLinkUpdateRequest request)
        {
            var existing = await _repo.GetByIdAsync(linkId)
                ?? throw new KeyNotFoundException("Link not found");

            if (request.category != null) existing.category = request.category;
            if (request.title != null) existing.title = request.title;
            if (request.url != null) existing.url = request.url;
            if (request.link_type != null) existing.link_type = request.link_type.ToUpper();
            if (request.description != null) existing.description = request.description;
            existing.status = "PENDING"; // reset on edit
            existing.updated_at = DateTime.UtcNow;

            await _repo.UpdateAsync(existing);
            return ApiResponseData<object>.Success(null, "Link updated successfully");
        }

        public async Task<ApiResponseData<List<StepLinkDto>>> GetByStepIdAsync(Guid taskWorkflowStepId)
        {
            var links = await _repo.GetByStepIdAsync(taskWorkflowStepId);
            return ApiResponseData<List<StepLinkDto>>.Success(links);
        }

        public async Task<ApiResponseData<List<StepLinkDto>>> GetByTaskIdAsync(Guid taskId)
        {
            var links = await _repo.GetByTaskIdAsync(taskId);
            return ApiResponseData<List<StepLinkDto>>.Success(links);
        }

        public async Task<ApiResponseData<object>> ReviewAsync(Guid linkId, StepLinkReviewRequest request, Guid reviewedBy)
        {
            _ = await _repo.GetByIdAsync(linkId)
                ?? throw new KeyNotFoundException("Link not found");

            var valid = new[] { "PENDING", "APPROVED", "REJECTED" };
            if (!valid.Contains(request.status.ToUpper()))
                throw new ArgumentException("Status must be PENDING, APPROVED, or REJECTED");

            await _repo.ReviewAsync(linkId, request.status.ToUpper(), request.status_note, reviewedBy);
            return ApiResponseData<object>.Success(null, $"Link marked as {request.status.ToUpper()}");
        }

        public async Task<ApiResponseData<object>> DeleteAsync(Guid linkId)
        {
            _ = await _repo.GetByIdAsync(linkId)
                ?? throw new KeyNotFoundException("Link not found");

            await _repo.DeleteAsync(linkId);
            return ApiResponseData<object>.Success(null, "Link deleted");
        }
    }
}

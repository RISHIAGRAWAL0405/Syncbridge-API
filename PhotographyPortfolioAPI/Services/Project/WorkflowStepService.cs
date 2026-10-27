using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Contracts.Models.Project;
using PhotographyPortfolioAPI.Infrastructure.Interfaces.Project;
using PhotographyPortfolioAPI.Interfaces.Project;

namespace PhotographyPortfolioAPI.Services.Project
{
    public class WorkflowStepService : IWorkflowStepService
    {
        private readonly IWorkflowStepRepository _repo;

        public WorkflowStepService(IWorkflowStepRepository repo) => _repo = repo;

        public async Task<ApiResponseData<object>> CreateAsync(WorkflowStepCreateRequest request)
        {
            var step = new WorkflowStepMaster
            {
                workflow_step_id = Guid.NewGuid(),
                workflow_step_name = request.workflow_step_name,
                workflow_step_code = request.workflow_step_code,
                description = request.description,
                is_active = true,
                created_at = DateTime.UtcNow
            };
            var id = await _repo.CreateAsync(step);
            return ApiResponseData<object>.Success(null, "Workflow step created successfully", id);
        }

        public async Task<ApiResponseData<object>> UpdateAsync(Guid stepId, WorkflowStepUpdateRequest request)
        {
            var step = await _repo.GetByIdAsync(stepId);
            if (step == null) throw new KeyNotFoundException("Workflow step not found");

            if (!string.IsNullOrWhiteSpace(request.workflow_step_name)) step.workflow_step_name = request.workflow_step_name;
            if (!string.IsNullOrWhiteSpace(request.description)) step.description = request.description;
            step.updated_at = DateTime.UtcNow;

            await _repo.UpdateAsync(step);
            return ApiResponseData<object>.Success(null, "Workflow step updated successfully", stepId);
        }

        public async Task<ApiResponseData<WorkflowStepDto>> GetByIdAsync(Guid stepId)
        {
            var step = await _repo.GetByIdAsync(stepId);
            if (step == null) throw new KeyNotFoundException("Workflow step not found");
            var dto = new WorkflowStepDto
            {
                workflow_step_id = step.workflow_step_id,
                workflow_step_name = step.workflow_step_name,
                workflow_step_code = step.workflow_step_code,
                description = step.description,
                is_active = step.is_active,
                created_at = step.created_at
            };
            return ApiResponseData<WorkflowStepDto>.Success(dto, "Workflow step retrieved successfully");
        }

        public async Task<ApiResponseData<List<WorkflowStepDto>>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return ApiResponseData<List<WorkflowStepDto>>.Success(list, "Workflow steps retrieved successfully");
        }

        public async Task<ApiResponseData<List<WorkflowStepDto>>> GetAllActiveAsync()
        {
            var list = await _repo.GetAllActiveAsync();
            return ApiResponseData<List<WorkflowStepDto>>.Success(list, "Workflow steps retrieved successfully");
        }

        public async Task<ApiResponseData<object>> ActiveInactiveAsync(Guid stepId, bool isActive)
        {
            var step = await _repo.GetByIdAsync(stepId);
            if (step == null) throw new KeyNotFoundException("Workflow step not found");
            await _repo.ActiveInactiveAsync(stepId, isActive);
            return ApiResponseData<object>.Success(new { id = stepId }, "Workflow step status updated successfully");
        }
    }
}

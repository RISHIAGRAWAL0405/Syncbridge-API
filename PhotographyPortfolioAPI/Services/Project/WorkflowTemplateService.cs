using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Contracts.Models.Project;
using PhotographyPortfolioAPI.Infrastructure.Interfaces.Project;
using PhotographyPortfolioAPI.Interfaces.Project;

namespace PhotographyPortfolioAPI.Services.Project
{
    public class WorkflowTemplateService : IWorkflowTemplateService
    {
        private readonly IWorkflowTemplateRepository _repo;

        public WorkflowTemplateService(IWorkflowTemplateRepository repo) => _repo = repo;

        public async Task<ApiResponseData<object>> CreateAsync(WorkflowTemplateCreateRequest request)
        {
            var templateId = Guid.NewGuid();
            var template = new WorkflowTemplate
            {
                workflow_template_id = templateId,
                workflow_template_name = request.workflow_template_name,
                workflow_template_code = request.workflow_template_code,
                description = request.description,
                is_active = true,
                created_at = DateTime.UtcNow
            };
            await _repo.CreateAsync(template);

            foreach (var s in request.steps)
            {
                await _repo.AddStepAsync(new WorkflowTemplateStep
                {
                    workflow_template_step_id = Guid.NewGuid(),
                    workflow_template_id = templateId,
                    workflow_step_id = s.workflow_step_id,
                    display_order = s.display_order,
                    is_required = s.is_required,
                    created_at = DateTime.UtcNow
                });
            }

            return ApiResponseData<object>.Success(null, "Workflow template created successfully", templateId);
        }

        public async Task<ApiResponseData<object>> UpdateAsync(Guid templateId, WorkflowTemplateUpdateRequest request)
        {
            var template = await _repo.GetByIdAsync(templateId);
            if (template == null) throw new KeyNotFoundException("Workflow template not found");

            if (!string.IsNullOrWhiteSpace(request.workflow_template_name)) template.workflow_template_name = request.workflow_template_name;
            if (!string.IsNullOrWhiteSpace(request.description)) template.description = request.description;
            template.updated_at = DateTime.UtcNow;

            await _repo.UpdateAsync(template);
            return ApiResponseData<object>.Success(null, "Workflow template updated successfully", templateId);
        }

        public async Task<ApiResponseData<WorkflowTemplateDto>> GetByIdAsync(Guid templateId)
        {
            var dto = await _repo.GetWithStepsByIdAsync(templateId);
            if (dto == null) throw new KeyNotFoundException("Workflow template not found");
            return ApiResponseData<WorkflowTemplateDto>.Success(dto, "Workflow template retrieved successfully");
        }

        public async Task<ApiResponseData<List<WorkflowTemplateDto>>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return ApiResponseData<List<WorkflowTemplateDto>>.Success(list, "Workflow templates retrieved successfully");
        }

        public async Task<ApiResponseData<object>> ActiveInactiveAsync(Guid templateId, bool isActive)
        {
            var template = await _repo.GetByIdAsync(templateId);
            if (template == null) throw new KeyNotFoundException("Workflow template not found");
            await _repo.ActiveInactiveAsync(templateId, isActive);
            return ApiResponseData<object>.Success(new { id = templateId }, "Workflow template status updated successfully");
        }

        public async Task<ApiResponseData<object>> UpdateStepsAsync(Guid templateId, List<WorkflowTemplateStepRequest> steps)
        {
            var template = await _repo.GetByIdAsync(templateId);
            if (template == null) throw new KeyNotFoundException("Workflow template not found");

            await _repo.RemoveAllStepsAsync(templateId);
            foreach (var s in steps)
            {
                await _repo.AddStepAsync(new WorkflowTemplateStep
                {
                    workflow_template_step_id = Guid.NewGuid(),
                    workflow_template_id = templateId,
                    workflow_step_id = s.workflow_step_id,
                    display_order = s.display_order,
                    is_required = s.is_required,
                    created_at = DateTime.UtcNow
                });
            }
            return ApiResponseData<object>.Success(null, "Workflow template steps updated successfully");
        }

        public async Task<ApiResponseData<List<WorkflowTemplateStepDto>>> GetStepsAsync(Guid templateId)
        {
            var steps = await _repo.GetStepsByTemplateIdAsync(templateId);
            return ApiResponseData<List<WorkflowTemplateStepDto>>.Success(steps, "Steps retrieved successfully");
        }
    }
}

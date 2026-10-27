using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Interfaces.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PhotographyPortfolioAPI.Controllers.Project
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkflowTemplatesController : ControllerBase
    {
        private readonly IWorkflowTemplateService _service;

        public WorkflowTemplatesController(IWorkflowTemplateService service) => _service = service;

        [HttpPost]
        public async Task<ApiResponseData<object>> Create([FromBody] WorkflowTemplateCreateRequest request)
        {
            try { return await _service.CreateAsync(request); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred while creating workflow template", 500); }
        }

        [HttpPut("{template_id}")]
        public async Task<ApiResponseData<object>> Update(Guid template_id, [FromBody] WorkflowTemplateUpdateRequest request)
        {
            try { return await _service.UpdateAsync(template_id, request); }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred while updating workflow template", 500); }
        }

        [HttpGet("{template_id}")]
        public async Task<ApiResponseData<WorkflowTemplateDto>> GetById(Guid template_id)
        {
            try { return await _service.GetByIdAsync(template_id); }
            catch (KeyNotFoundException ex) { return ApiResponseData<WorkflowTemplateDto>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<WorkflowTemplateDto>.Error("An error occurred", 500); }
        }

        [HttpGet]
        public async Task<ApiResponseData<List<WorkflowTemplateDto>>> GetAll()
        {
            try { return await _service.GetAllAsync(); }
            catch (Exception) { return ApiResponseData<List<WorkflowTemplateDto>>.Error("An error occurred", 500); }
        }

        [HttpPatch("{template_id}/status")]
        public async Task<ApiResponseData<object>> ActiveInactive(Guid template_id, [FromQuery] bool is_active)
        {
            try { return await _service.ActiveInactiveAsync(template_id, is_active); }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred", 500); }
        }

        [HttpPut("{template_id}/steps")]
        public async Task<ApiResponseData<object>> UpdateSteps(Guid template_id, [FromBody] List<WorkflowTemplateStepRequest> steps)
        {
            try { return await _service.UpdateStepsAsync(template_id, steps); }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred while updating steps", 500); }
        }

        [HttpGet("{template_id}/steps")]
        public async Task<ApiResponseData<List<WorkflowTemplateStepDto>>> GetSteps(Guid template_id)
        {
            try { return await _service.GetStepsAsync(template_id); }
            catch (Exception) { return ApiResponseData<List<WorkflowTemplateStepDto>>.Error("An error occurred", 500); }
        }
    }
}

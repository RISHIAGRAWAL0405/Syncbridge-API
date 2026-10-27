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
    public class WorkflowStepsController : ControllerBase
    {
        private readonly IWorkflowStepService _service;

        public WorkflowStepsController(IWorkflowStepService service) => _service = service;

        [HttpPost]
        public async Task<ApiResponseData<object>> Create([FromBody] WorkflowStepCreateRequest request)
        {
            try { return await _service.CreateAsync(request); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred while creating workflow step", 500); }
        }

        [HttpPut("{step_id}")]
        public async Task<ApiResponseData<object>> Update(Guid step_id, [FromBody] WorkflowStepUpdateRequest request)
        {
            try { return await _service.UpdateAsync(step_id, request); }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred while updating workflow step", 500); }
        }

        [HttpGet("{step_id}")]
        public async Task<ApiResponseData<WorkflowStepDto>> GetById(Guid step_id)
        {
            try { return await _service.GetByIdAsync(step_id); }
            catch (KeyNotFoundException ex) { return ApiResponseData<WorkflowStepDto>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<WorkflowStepDto>.Error("An error occurred", 500); }
        }

        [HttpGet]
        public async Task<ApiResponseData<List<WorkflowStepDto>>> GetAll()
        {
            try { return await _service.GetAllAsync(); }
            catch (Exception) { return ApiResponseData<List<WorkflowStepDto>>.Error("An error occurred", 500); }
        }

        [HttpGet("active")]
        public async Task<ApiResponseData<List<WorkflowStepDto>>> GetAllActive()
        {
            try { return await _service.GetAllActiveAsync(); }
            catch (Exception) { return ApiResponseData<List<WorkflowStepDto>>.Error("An error occurred", 500); }
        }

        [HttpPatch("{step_id}/status")]
        public async Task<ApiResponseData<object>> ActiveInactive(Guid step_id, [FromQuery] bool is_active)
        {
            try { return await _service.ActiveInactiveAsync(step_id, is_active); }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred", 500); }
        }
    }
}

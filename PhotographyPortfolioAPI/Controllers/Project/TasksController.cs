using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Extensions;
using PhotographyPortfolioAPI.Interfaces.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PhotographyPortfolioAPI.Controllers.Project
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _service;

        public TasksController(ITaskService service) => _service = service;

        [HttpPost]
        public async Task<ApiResponseData<object>> Create([FromBody] TaskCreateRequest request)
        {
            try { return await _service.CreateAsync(request); }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred while creating task", 500); }
        }

        [HttpPut("{task_id}")]
        public async Task<ApiResponseData<object>> Update(Guid task_id, [FromBody] TaskUpdateRequest request)
        {
            try { return await _service.UpdateAsync(task_id, request); }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred while updating task", 500); }
        }

        [HttpGet("{task_id}")]
        public async Task<ApiResponseData<TaskDto>> GetById(Guid task_id)
        {
            try { return await _service.GetByIdAsync(task_id); }
            catch (KeyNotFoundException ex) { return ApiResponseData<TaskDto>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<TaskDto>.Error("An error occurred", 500); }
        }

        [HttpGet]
        public async Task<ApiResponseData<List<TaskDto>>> GetAll()
        {
            try { return await _service.GetAllAsync(); }
            catch (Exception ex) { return ApiResponseData<List<TaskDto>>.Error("An error occurred", 500); }
        }

        [HttpGet("by-project/{project_id}")]
        public async Task<ApiResponseData<List<TaskDto>>> GetByProject(Guid project_id)
        {
            try { return await _service.GetByProjectIdAsync(project_id); }
            catch (Exception) { return ApiResponseData<List<TaskDto>>.Error("An error occurred", 500); }
        }

        [HttpDelete("{task_id}")]
        public async Task<ApiResponseData<object>> Delete(Guid task_id)
        {
            try { return await _service.DeleteAsync(task_id); }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred while deleting task", 500); }
        }

        [HttpPatch("{task_id}/task-status")]
        public async Task<ApiResponseData<object>> UpdateStatus(Guid task_id, [FromBody] TaskStatusUpdateRequest request)
        {
            try { return await _service.UpdateStatusAsync(task_id, request); }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (InvalidOperationException ex) { return ApiResponseData<object>.Error(ex.Message, 400); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred while updating task status", 500); }
        }

        [HttpPut("{task_id}/workflow-steps/{task_workflow_step_id}")]
        public async Task<ApiResponseData<object>> UpdateWorkflowStep(Guid task_id, Guid task_workflow_step_id, [FromBody] TaskWorkflowStepUpdateRequest request)
        {
            try { return await _service.UpdateWorkflowStepAsync(task_id, task_workflow_step_id, request); }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred while updating workflow step", 500); }
        }

        [HttpPut("{task_id}/workflow-steps/schedule")]
        public async Task<ApiResponseData<object>> UpdateWorkflowStepsSchedule(Guid task_id, [FromBody] TaskWorkflowStepsScheduleRequest request)
        {
            try { return await _service.UpdateWorkflowStepsScheduleAsync(task_id, request); }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred while updating workflow steps schedule", 500); }
        }

        [HttpPut("{task_id}/workflow-steps/{task_workflow_step_id}/status")]
        public async Task<ApiResponseData<object>> UpdateWorkflowStepStatus(Guid task_id, Guid task_workflow_step_id, [FromBody] TaskWorkflowStepUpdateRequestStatus request)
        {
            try { return await _service.UpdateWorkflowStepStatusAsync(task_id, task_workflow_step_id, request.status); }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (InvalidOperationException ex) { return ApiResponseData<object>.Error(ex.Message, 400); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred while updating workflow step status", 500); }
        }

        [HttpGet("calendar")]
        public async Task<ApiResponseData<List<CalendarTaskDto>>> GetCalendarTasks([FromQuery] int year, [FromQuery] int month)
        {
            try
            {
                var clientId = User.GetUserId();
                if (year == 0) year = DateTime.UtcNow.Year;
                if (month == 0) month = DateTime.UtcNow.Month;
                return await _service.GetCalendarTasksAsync(clientId, year, month);
            }
            catch (Exception) { return ApiResponseData<List<CalendarTaskDto>>.Error("An error occurred", 500); }
        }
    }
}

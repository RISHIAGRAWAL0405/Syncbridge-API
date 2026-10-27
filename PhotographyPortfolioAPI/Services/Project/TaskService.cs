using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Contracts.Models.Project;
using PhotographyPortfolioAPI.Infrastructure.Interfaces.Project;
using PhotographyPortfolioAPI.Interfaces.Project;

namespace PhotographyPortfolioAPI.Services.Project
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repo;
        private readonly IProjectRepository _projectRepo;
        private readonly IWorkflowTemplateRepository _templateRepo;
        private readonly IWorkflowStepRepository _stepRepo;

        public TaskService(
            ITaskRepository repo,
            IProjectRepository projectRepo,
            IWorkflowTemplateRepository templateRepo,
            IWorkflowStepRepository stepRepo)
        {
            _repo = repo;
            _projectRepo = projectRepo;
            _templateRepo = templateRepo;
            _stepRepo = stepRepo;
        }

        public async Task<ApiResponseData<object>> CreateAsync(TaskCreateRequest request)
        {
            // Validate project exists
            var project = await _projectRepo.GetByIdAsync(request.project_id);
            if (project == null) throw new KeyNotFoundException("Project not found");

            // Validate workflow template exists
            var template = await _templateRepo.GetByIdAsync(request.workflow_template_id);
            if (template == null) throw new KeyNotFoundException("Workflow template not found");

            // Create task
            var task = new TaskMaster
            {
                task_id = Guid.NewGuid(),
                project_id = request.project_id,
                workflow_template_id = request.workflow_template_id,
                task_name = request.task_name,
                task_code = request.task_code,
                description = request.description,
                start_date = request.start_date,
                end_date = request.end_date,
                is_active = true,
                status = "NEW",
                created_at = DateTime.UtcNow
            };

            // Get all steps from workflow template
            var templateSteps = await _templateRepo.GetStepsByTemplateIdAsync(request.workflow_template_id);

            // Create task workflow steps with same display order
            var workflowSteps = templateSteps.Select(step => new TaskWorkflowStep
            {
                task_workflow_step_id = Guid.NewGuid(),
                task_id = task.task_id,
                workflow_step_id = step.workflow_step_id,
                step_name = step.workflow_step_name,
                step_code = step.workflow_step_code,
                display_order = step.display_order,
                is_required = step.is_required,
                start_date = null,
                end_date = null,
                notes = null,
                created_at = DateTime.UtcNow
            }).ToList();

            // Create task and workflow steps in transaction
            var id = await _repo.CreateAsync(task, workflowSteps);
            return ApiResponseData<object>.Success(null, "Task created successfully", id);
        }

        public async Task<ApiResponseData<object>> UpdateAsync(Guid taskId, TaskUpdateRequest request)
        {
            var task = await _repo.GetByIdAsync(taskId);
            if (task == null) throw new KeyNotFoundException("Task not found");

            if (!string.IsNullOrWhiteSpace(request.task_name)) task.task_name = request.task_name;
            if (!string.IsNullOrWhiteSpace(request.task_code)) task.task_code = request.task_code;
            if (!string.IsNullOrWhiteSpace(request.description)) task.description = request.description;
            if (request.start_date.HasValue) task.start_date = request.start_date;
            if (request.end_date.HasValue) task.end_date = request.end_date;
            task.updated_at = DateTime.UtcNow;

            await _repo.UpdateAsync(task);
            return ApiResponseData<object>.Success(null, "Task updated successfully", taskId);
        }

        public async Task<ApiResponseData<TaskDto>> GetByIdAsync(Guid taskId)
        {
            var task = await _repo.GetWithWorkflowStepsByIdAsync(taskId);
            if (task == null) throw new KeyNotFoundException("Task not found");
            return ApiResponseData<TaskDto>.Success(task, "Task retrieved successfully");
        }

        public async Task<ApiResponseData<List<TaskDto>>> GetAllAsync()
        {
            var tasks = await _repo.GetAllAsync();
            return ApiResponseData<List<TaskDto>>.Success(tasks, "Tasks retrieved successfully");
        }

        public async Task<ApiResponseData<List<TaskDto>>> GetByProjectIdAsync(Guid projectId)
        {
            var tasks = await _repo.GetByProjectIdAsync(projectId);
            return ApiResponseData<List<TaskDto>>.Success(tasks, "Tasks retrieved successfully");
        }

        public async Task<ApiResponseData<object>> DeleteAsync(Guid taskId)
        {
            var task = await _repo.GetByIdAsync(taskId);
            if (task == null) throw new KeyNotFoundException("Task not found");

            await _repo.DeleteAsync(taskId);
            return ApiResponseData<object>.Success(new { id = taskId }, "Task deleted successfully");
        }

        public async Task<ApiResponseData<object>> UpdateStatusAsync(Guid taskId, TaskStatusUpdateRequest request)
        {
            var task = await _repo.GetByIdAsync(taskId);
            if (task == null) throw new KeyNotFoundException("Task not found");

            if (task.status == "COMPLETED" || task.status == "CANCELLED")
                throw new InvalidOperationException("Task status cannot be changed once it is Completed or Cancelled");

            await _repo.UpdateStatusAsync(taskId, request.status.ToString());
            return ApiResponseData<object>.Success(new { id = taskId, status = request.status.ToString() }, "Task status updated successfully");
        }

        public async Task<ApiResponseData<object>> UpdateWorkflowStepAsync(Guid taskId, Guid taskWorkflowStepId, TaskWorkflowStepUpdateRequest request)
        {
            var task = await _repo.GetByIdAsync(taskId);
            if (task == null) throw new KeyNotFoundException("Task not found");

            var steps = await _repo.GetWorkflowStepsByTaskIdAsync(taskId);
            var step = steps.FirstOrDefault(s => s.task_workflow_step_id == taskWorkflowStepId);
            if (step == null) throw new KeyNotFoundException("Workflow step not found");

            if (request.start_date.HasValue) step.start_date = request.start_date;
            if (request.end_date.HasValue) step.end_date = request.end_date;
            if (!string.IsNullOrWhiteSpace(request.notes)) step.notes = request.notes;
            step.updated_at = DateTime.UtcNow;

            await _repo.UpdateWorkflowStepAsync(step);
            return ApiResponseData<object>.Success(new { id = taskWorkflowStepId }, "Workflow step updated successfully");
        }

        public async Task<ApiResponseData<object>> UpdateWorkflowStepsScheduleAsync(Guid taskId, TaskWorkflowStepsScheduleRequest request)
        {
            var task = await _repo.GetByIdAsync(taskId);
            if (task == null) throw new KeyNotFoundException("Task not found");

            var steps = await _repo.GetWorkflowStepsByTaskIdAsync(taskId);

            var stepsToUpdate = new List<TaskWorkflowStep>();
            foreach (var item in request.steps)
            {
                var step = steps.FirstOrDefault(s => s.task_workflow_step_id == item.task_workflow_step_id);
                if (step == null) throw new KeyNotFoundException($"Workflow step {item.task_workflow_step_id} not found");

                if (item.start_date.HasValue) step.start_date = item.start_date;
                if (item.end_date.HasValue) step.end_date = item.end_date;
                if (!string.IsNullOrWhiteSpace(item.notes)) step.notes = item.notes;
                step.updated_at = DateTime.UtcNow;

                stepsToUpdate.Add(step);
            }

            await _repo.UpdateMultipleWorkflowStepsAsync(stepsToUpdate);
            return ApiResponseData<object>.Success(null, "Workflow steps schedule updated successfully");
        }

        public async Task<ApiResponseData<object>> UpdateWorkflowStepStatusAsync(Guid taskId, Guid taskWorkflowStepId, string status)
        {
            var task = await _repo.GetByIdAsync(taskId);
            if (task == null) throw new KeyNotFoundException("Task not found");

            if (task.status == "COMPLETED" || task.status == "CANCELLED")
                throw new InvalidOperationException("Workflow steps cannot be changed once the task is Completed or Cancelled");

            var steps = await _repo.GetWorkflowStepsByTaskIdAsync(taskId);
            var step = steps.FirstOrDefault(s => s.task_workflow_step_id == taskWorkflowStepId);
            if (step == null) throw new KeyNotFoundException("Workflow step not found");

            var validStatuses = new[] { "NOT_STARTED", "IN_PROGRESS", "COMPLETED", "SKIPPED" };
            if (!validStatuses.Contains(status)) throw new KeyNotFoundException("Invalid status value");

            // transitions
            if (status == "IN_PROGRESS")
            {
                if (step.started_at == null) step.started_at = DateTime.UtcNow;
            }
            if (status == "COMPLETED")
            {
                step.completed_at = DateTime.UtcNow;
            }
            if (step.status == "COMPLETED" && status == "IN_PROGRESS")
            {
                step.completed_at = null;
            }

            step.status = status;
            step.updated_at = DateTime.UtcNow;

            await _repo.UpdateWorkflowStepAsync(step);

            // Auto-complete the task once every workflow step is finished (completed or skipped)
            var allSteps = steps.Select(s => s.task_workflow_step_id == taskWorkflowStepId ? step : s).ToList();
            var allDone = allSteps.Count > 0 && allSteps.All(s => s.status == "COMPLETED" || s.status == "SKIPPED");
            if (allDone && task.status != "COMPLETED")
            {
                await _repo.UpdateStatusAsync(taskId, "COMPLETED");
            }

            return ApiResponseData<object>.Success(new { id = taskWorkflowStepId }, "Workflow step status updated successfully");
        }

        public async Task<ApiResponseData<List<CalendarTaskDto>>> GetCalendarTasksAsync(Guid clientId, int year, int month)
        {
            var tasks = await _repo.GetCalendarTasksAsync(clientId, year, month);
            return ApiResponseData<List<CalendarTaskDto>>.Success(tasks, "Calendar tasks retrieved successfully");
        }
    }
}

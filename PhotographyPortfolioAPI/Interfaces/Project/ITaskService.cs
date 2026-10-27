using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;

namespace PhotographyPortfolioAPI.Interfaces.Project
{
    public interface ITaskService
    {
        Task<ApiResponseData<object>> CreateAsync(TaskCreateRequest request);
        Task<ApiResponseData<object>> UpdateAsync(Guid taskId, TaskUpdateRequest request);
        Task<ApiResponseData<TaskDto>> GetByIdAsync(Guid taskId);
        Task<ApiResponseData<List<TaskDto>>> GetAllAsync();
        Task<ApiResponseData<List<TaskDto>>> GetByProjectIdAsync(Guid projectId);
        Task<ApiResponseData<object>> DeleteAsync(Guid taskId);
        Task<ApiResponseData<object>> UpdateStatusAsync(Guid taskId, TaskStatusUpdateRequest request);
        Task<ApiResponseData<object>> UpdateWorkflowStepAsync(Guid taskId, Guid taskWorkflowStepId, TaskWorkflowStepUpdateRequest request);
        Task<ApiResponseData<object>> UpdateWorkflowStepsScheduleAsync(Guid taskId, TaskWorkflowStepsScheduleRequest request);
        Task<ApiResponseData<object>> UpdateWorkflowStepStatusAsync(Guid taskId, Guid taskWorkflowStepId, string status);
        Task<ApiResponseData<List<CalendarTaskDto>>> GetCalendarTasksAsync(Guid clientId, int year, int month);
    }
}

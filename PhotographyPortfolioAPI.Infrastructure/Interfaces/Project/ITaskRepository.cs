using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Contracts.Models.Project;

namespace PhotographyPortfolioAPI.Infrastructure.Interfaces.Project
{
    public interface ITaskRepository
    {
        Task<Guid> CreateAsync(TaskMaster task, List<TaskWorkflowStep> workflowSteps);
        Task UpdateAsync(TaskMaster task);
        Task UpdateStatusAsync(Guid taskId, string status);
        Task<TaskMaster?> GetByIdAsync(Guid taskId);
        Task<TaskDto?> GetWithWorkflowStepsByIdAsync(Guid taskId);
        Task<List<TaskDto>> GetAllAsync();
        Task<List<TaskDto>> GetByProjectIdAsync(Guid projectId);
        Task DeleteAsync(Guid taskId);
        Task<List<TaskWorkflowStep>> GetWorkflowStepsByTaskIdAsync(Guid taskId);
        Task<TaskWorkflowStep?> GetWorkflowStepByIdAsync(Guid taskWorkflowStepId);
        Task UpdateWorkflowStepAsync(TaskWorkflowStep step);
        Task UpdateMultipleWorkflowStepsAsync(List<TaskWorkflowStep> steps);
        Task<List<CalendarTaskDto>> GetCalendarTasksAsync(Guid clientId, int year, int month);
    }
}

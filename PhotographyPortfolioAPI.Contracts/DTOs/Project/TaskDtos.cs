using TaskStatus = PhotographyPortfolioAPI.Core.Enums.Project.TaskItemStatus;

namespace PhotographyPortfolioAPI.Contracts.DTOs.Project
{
    public class TaskCreateRequest
    {
        public Guid project_id { get; set; }
        public Guid workflow_template_id { get; set; }
        public string task_name { get; set; }
        public string task_code { get; set; }
        public string? description { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
    }

    public class TaskUpdateRequest
    {
        public string? task_name { get; set; }
        public string? task_code { get; set; }
        public string? description { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
    }

    public class TaskDto
    {
        public Guid task_id { get; set; }
        public Guid project_id { get; set; }
        public string project_name { get; set; }
        public Guid workflow_template_id { get; set; }
        public string workflow_template_name { get; set; }
        public string task_name { get; set; }
        public string task_code { get; set; }
        public string description { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public bool is_active { get; set; }
        public string status { get; set; }
        public DateTime created_at { get; set; }
        public List<TaskWorkflowStepDto> workflow_steps { get; set; } = new();
    }

    public class TaskStatusUpdateRequest
    {
        public TaskStatus status { get; set; }
    }

    public class TaskWorkflowStepDto
    {
        public Guid task_workflow_step_id { get; set; }
        public Guid task_id { get; set; }
        public Guid workflow_step_id { get; set; }
        public string step_name { get; set; }
        public string step_code { get; set; }
        public int display_order { get; set; }
        public bool is_required { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public string notes { get; set; }
        public string status { get; set; }
        public DateTime? started_at { get; set; }
        public DateTime? completed_at { get; set; }
        public List<StepLinkDto> links { get; set; } = new();
    }

    public class TaskWorkflowStepUpdateRequest
    {
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public string? notes { get; set; }
    }

    public class TaskWorkflowStepUpdateRequestStatus
    {
        public string status { get; set; }
    }

    public class TaskWorkflowStepsScheduleRequest
    {
        public List<TaskWorkflowStepScheduleItem> steps { get; set; } = new();
    }

    public class TaskWorkflowStepScheduleItem
    {
        public Guid task_workflow_step_id { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public string? notes { get; set; }
    }

    public class CalendarTaskDto
    {
        public Guid task_id { get; set; }
        public string task_name { get; set; }
        public string task_code { get; set; }
        public string description { get; set; }
        public Guid project_id { get; set; }
        public string project_name { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public string status { get; set; }
        public int total_steps { get; set; }
        public int completed_steps { get; set; }
        public List<TaskWorkflowStepDto> workflow_steps { get; set; } = new();
    }
}

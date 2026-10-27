namespace PhotographyPortfolioAPI.Contracts.DTOs.Project
{
    public class ProjectProfileDto
    {
        public ProjectHeaderDto project { get; set; }
        public ClientProfileDto client { get; set; }
        public ProjectSummaryDto summary { get; set; }
        public List<TaskProfileDto> tasks { get; set; } = new();
    }

    public class ProjectHeaderDto
    {
        public Guid project_id { get; set; }
        public string project_name { get; set; }
        public string project_code { get; set; }
        public string description { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public string status { get; set; }
        public bool is_active { get; set; }
    }

    public class ClientProfileDto
    {
        public Guid client_id { get; set; }
        public string client_name { get; set; }
        public string company_name { get; set; }
    }

    public class ProjectSummaryDto
    {
        public int total_tasks { get; set; }
        public int completed_tasks { get; set; }
        public int in_progress_tasks { get; set; }
        public int not_started_tasks { get; set; }
        public int overdue_tasks { get; set; }
        public int overall_progress { get; set; }
    }

    public class TaskProfileDto
    {
        public Guid task_id { get; set; }
        public string task_name { get; set; }
        public string task_code { get; set; }
        public string workflow_template_name { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public string current_stage { get; set; }
        public string status { get; set; }
        public TaskProgressDto progress { get; set; }
        public List<TaskWorkflowStepProfileDto> workflow_steps { get; set; } = new();
    }

    public class TaskProgressDto
    {
        public int total_steps { get; set; }
        public int completed_steps { get; set; }
        public int percentage { get; set; }
    }

    public class TaskWorkflowStepProfileDto
    {
        public Guid task_workflow_step_id { get; set; }
        public string step_name { get; set; }
        public int display_order { get; set; }
        public string status { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public List<StepLinkDto> links { get; set; } = new();
    }
}

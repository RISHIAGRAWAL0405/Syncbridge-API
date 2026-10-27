namespace PhotographyPortfolioAPI.Contracts.Models.Project
{
    public class TaskWorkflowStepLink
    {
        public Guid link_id { get; set; }
        public Guid task_workflow_step_id { get; set; }
        public Guid task_id { get; set; }
        public string category { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public string url { get; set; } = string.Empty;
        public string link_type { get; set; } = "VIDEO";
        public string? description { get; set; }
        public string status { get; set; } = "PENDING";
        public string? status_note { get; set; }
        public Guid? uploaded_by { get; set; }
        public Guid? reviewed_by { get; set; }
        public DateTime? reviewed_at { get; set; }
        public bool is_active { get; set; } = true;
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}

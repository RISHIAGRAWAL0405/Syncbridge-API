namespace PhotographyPortfolioAPI.Contracts.Models.Project
{
    public class TaskWorkflowStep
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
        public string status { get; set; } = "NOT_STARTED";
        public DateTime? started_at { get; set; }
        public DateTime? completed_at { get; set; }
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}

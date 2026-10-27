namespace PhotographyPortfolioAPI.Contracts.Models.Project
{
    public class TaskMaster
    {
        public Guid task_id { get; set; }
        public Guid project_id { get; set; }
        public Guid workflow_template_id { get; set; }
        public string task_name { get; set; }
        public string task_code { get; set; }
        public string description { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public bool is_active { get; set; }
        public string status { get; set; } = "NEW";
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}

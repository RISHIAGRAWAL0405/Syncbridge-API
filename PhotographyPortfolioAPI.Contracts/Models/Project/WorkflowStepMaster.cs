namespace PhotographyPortfolioAPI.Contracts.Models.Project
{
    public class WorkflowStepMaster
    {
        public Guid workflow_step_id { get; set; }
        public string workflow_step_name { get; set; }
        public string workflow_step_code { get; set; }
        public string description { get; set; }
        public bool is_active { get; set; }
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}

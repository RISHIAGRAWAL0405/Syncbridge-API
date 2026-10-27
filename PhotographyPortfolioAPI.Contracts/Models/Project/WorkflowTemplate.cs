namespace PhotographyPortfolioAPI.Contracts.Models.Project
{
    public class WorkflowTemplate
    {
        public Guid workflow_template_id { get; set; }
        public string workflow_template_name { get; set; }
        public string workflow_template_code { get; set; }
        public string description { get; set; }
        public bool is_active { get; set; }
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}

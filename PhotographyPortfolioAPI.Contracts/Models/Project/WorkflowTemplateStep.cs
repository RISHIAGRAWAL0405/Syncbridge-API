namespace PhotographyPortfolioAPI.Contracts.Models.Project
{
    public class WorkflowTemplateStep
    {
        public Guid workflow_template_step_id { get; set; }
        public Guid workflow_template_id { get; set; }
        public Guid workflow_step_id { get; set; }
        public int display_order { get; set; }
        public bool is_required { get; set; }
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}

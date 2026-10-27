namespace PhotographyPortfolioAPI.Contracts.DTOs.Project
{
    public class WorkflowStepCreateRequest
    {
        public string workflow_step_name { get; set; }
        public string workflow_step_code { get; set; }
        public string? description { get; set; }
    }

    public class WorkflowStepUpdateRequest
    {
        public string? workflow_step_name { get; set; }
        public string? description { get; set; }
    }

    public class WorkflowStepDto
    {
        public Guid workflow_step_id { get; set; }
        public string workflow_step_name { get; set; }
        public string workflow_step_code { get; set; }
        public string description { get; set; }
        public bool is_active { get; set; }
        public DateTime created_at { get; set; }
    }
}

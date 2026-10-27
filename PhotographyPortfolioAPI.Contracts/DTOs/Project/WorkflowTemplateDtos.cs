namespace PhotographyPortfolioAPI.Contracts.DTOs.Project
{
    public class WorkflowTemplateCreateRequest
    {
        public string workflow_template_name { get; set; }
        public string workflow_template_code { get; set; }
        public string? description { get; set; }
        public List<WorkflowTemplateStepRequest> steps { get; set; } = new();
    }

    public class WorkflowTemplateUpdateRequest
    {
        public string? workflow_template_name { get; set; }
        public string? description { get; set; }
    }

    public class WorkflowTemplateStepRequest
    {
        public Guid workflow_step_id { get; set; }
        public int display_order { get; set; }
        public bool is_required { get; set; } = true;
    }

    public class WorkflowTemplateDto
    {
        public Guid workflow_template_id { get; set; }
        public string workflow_template_name { get; set; }
        public string workflow_template_code { get; set; }
        public string description { get; set; }
        public bool is_active { get; set; }
        public DateTime created_at { get; set; }
        public List<WorkflowTemplateStepDto> steps { get; set; } = new();
    }

    public class WorkflowTemplateStepDto
    {
        public Guid workflow_template_step_id { get; set; }
        public Guid workflow_step_id { get; set; }
        public string workflow_step_name { get; set; }
        public string workflow_step_code { get; set; }
        public string description { get; set; }
        public int display_order { get; set; }
        public bool is_required { get; set; }
    }
}

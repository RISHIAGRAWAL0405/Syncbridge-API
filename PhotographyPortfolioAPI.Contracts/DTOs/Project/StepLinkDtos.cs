namespace PhotographyPortfolioAPI.Contracts.DTOs.Project
{
    public class StepLinkCreateRequest
    {
        public string category { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public string url { get; set; } = string.Empty;
        public string link_type { get; set; } = "VIDEO";   // VIDEO | IMAGE | FILE | OTHER
        public string? description { get; set; }
    }

    public class StepLinkUpdateRequest
    {
        public string? category { get; set; }
        public string? title { get; set; }
        public string? url { get; set; }
        public string? link_type { get; set; }
        public string? description { get; set; }
    }

    public class StepLinkReviewRequest
    {
        public string status { get; set; } = "PENDING";    // APPROVED | REJECTED | PENDING
        public string? status_note { get; set; }
    }

    public class StepLinkDto
    {
        public Guid link_id { get; set; }
        public Guid task_workflow_step_id { get; set; }
        public Guid task_id { get; set; }
        public string category { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public string url { get; set; } = string.Empty;
        public string link_type { get; set; } = string.Empty;
        public string? description { get; set; }
        public string status { get; set; } = string.Empty;
        public string? status_note { get; set; }
        public Guid? uploaded_by { get; set; }
        public Guid? reviewed_by { get; set; }
        public DateTime? reviewed_at { get; set; }
        public DateTime created_at { get; set; }
    }
}

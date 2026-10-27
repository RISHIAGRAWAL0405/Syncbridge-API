namespace PhotographyPortfolioAPI.Contracts.DTOs.Project
{
    public class ClientDashboardDto
    {
        public int TotalProjects { get; set; }
        public int ActiveProjects { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int OverdueTasks { get; set; }
        public int PendingApprovals { get; set; }
        public int OverallProgress { get; set; }
        public List<ClientProjectSummary> Projects { get; set; } = new();
        public List<ClientPendingLinkItem> PendingLinks { get; set; } = new();
    }

    public class ClientProjectSummary
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int Progress { get; set; }
    }

    public class ClientPendingLinkItem
    {
        public Guid LinkId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string LinkType { get; set; }
        public string Category { get; set; }
        public string StepName { get; set; }
        public string TaskName { get; set; }
        public string ProjectName { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

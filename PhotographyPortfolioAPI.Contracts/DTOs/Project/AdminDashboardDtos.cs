namespace PhotographyPortfolioAPI.Contracts.DTOs.Project
{
    public class AdminDashboardDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int TotalClients { get; set; }
        public int ActiveClients { get; set; }
        public int TotalProjects { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int OverdueTasks { get; set; }
        public int PendingLinks { get; set; }
        public int OverallProgress { get; set; }
        public List<AdminStatusCount> ProjectsByStatus { get; set; } = new();
        public List<AdminStatusCount> TasksByStatus { get; set; } = new();
        public List<AdminRecentProjectItem> RecentProjects { get; set; } = new();
        public List<AdminPendingLinkItem> PendingLinkItems { get; set; } = new();
    }

    public class AdminStatusCount
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }

    public class AdminRecentProjectItem
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string ClientName { get; set; }
        public string Status { get; set; }
        public int Progress { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class AdminPendingLinkItem
    {
        public Guid LinkId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string LinkType { get; set; }
        public string StepName { get; set; }
        public string TaskName { get; set; }
        public string ProjectName { get; set; }
        public string ClientName { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

using PhotographyPortfolioAPI.Contracts.DTOs.Project;

namespace PhotographyPortfolioAPI.Models
{
    // Keep for backward compat
    public class DashboardDataDto
    {
        public int ProjectsCount { get; set; }
        public int UpcomingTasks { get; set; }
        public int CompletedTasks { get; set; }
        public List<CalendarTaskDto> RecentTasks { get; set; } = new();
    }
}

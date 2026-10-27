using PhotographyPortfolioAPI.Core.Enums.Project;

namespace PhotographyPortfolioAPI.Contracts.DTOs.Project
{
    public class ProjectCreateRequest
    {
        public Guid client_id { get; set; }
        public string project_name { get; set; }
        public string? project_code { get; set; }
        public string? description { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public ProjectStatus? status { get; set; }
    }

    public class ProjectUpdateRequest
    {
        public string? project_name { get; set; }
        public string? project_code { get; set; }
        public string? description { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public ProjectStatus? status { get; set; }
    }

    public class ProjectStatusUpdateRequest
    {
        public ProjectStatus status { get; set; }
    }

    public class ProjectDto
    {
        public Guid project_id { get; set; }
        public Guid client_id { get; set; }
        public string client_name { get; set; }
        public string project_name { get; set; }
        public string project_code { get; set; }
        public string description { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public string status { get; set; }
        public bool is_active { get; set; }
        public DateTime created_at { get; set; }
    }
}

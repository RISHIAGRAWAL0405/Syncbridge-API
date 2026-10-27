namespace PhotographyPortfolioAPI.Contracts.Models.Project
{
    public class ProjectMaster
    {
        public Guid project_id { get; set; }
        public Guid client_id { get; set; }
        public string project_name { get; set; }
        public string project_code { get; set; }
        public string description { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public string status { get; set; }
        public bool is_active { get; set; }
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}

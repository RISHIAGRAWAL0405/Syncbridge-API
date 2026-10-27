namespace PhotographyPortfolioAPI.Contracts.DTOs.Project
{
    public class ClientCreateRequest
    {
        public Guid client_id { get; set; }   // user_id from UI — client IS a user
        public string client_name { get; set; }
        public string email { get; set; }
        public string? mobile { get; set; }
        public string? company_name { get; set; }
        public string? address { get; set; }
    }

    public class ClientUpdateRequest
    {
        public string? client_name { get; set; }
        public string? email { get; set; }
        public string? mobile { get; set; }
        public string? company_name { get; set; }
        public string? address { get; set; }
    }

    public class ClientDto
    {
        public Guid client_id { get; set; }
        public string client_name { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string company_name { get; set; }
        public string address { get; set; }
        public bool is_active { get; set; }
        public DateTime created_at { get; set; }
        public int project_count { get; set; }
    }
}

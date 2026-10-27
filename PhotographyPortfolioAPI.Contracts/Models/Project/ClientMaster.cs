namespace PhotographyPortfolioAPI.Contracts.Models.Project
{
    public class ClientMaster
    {
        public Guid client_id { get; set; }   // same as user_id — comes from UI
        public string client_name { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string company_name { get; set; }
        public string address { get; set; }
        public bool is_active { get; set; }
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}

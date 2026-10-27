namespace PhotographyPortfolioAPI.Contracts.Models.Auth
{
    public class Role
    {
        public Guid role_id { get; set; }
        public string role_code { get; set; }
        public string role_name { get; set; }
        public bool is_assignable { get; set; } = true;
    }
}

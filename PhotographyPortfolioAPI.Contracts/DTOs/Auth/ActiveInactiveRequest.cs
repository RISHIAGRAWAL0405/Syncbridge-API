namespace PhotographyPortfolioAPI.Contracts.DTOs.Auth
{
    public class ActiveInactiveRequest
    {
        public Guid user_id { get; set; }
        public bool is_active { get; set; }
    }
}

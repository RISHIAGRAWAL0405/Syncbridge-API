namespace PhotographyPortfolioAPI.Contracts.Models.Auth
{
    public class RefreshToken
    {
        public Guid refresh_token_id { get; set; }
        public Guid user_id { get; set; }
        public string token { get; set; }
        public DateTime expires_at { get; set; }
        public bool is_revoked { get; set; }
    }
}

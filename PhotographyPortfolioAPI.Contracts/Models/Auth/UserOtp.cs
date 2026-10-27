namespace PhotographyPortfolioAPI.Contracts.Models.Auth
{
    public class UserOtp
    {
        public long otp_id { get; set; }
        public Guid user_id { get; set; }
        public string otp_code { get; set; }
        public string otp_type { get; set; }
        public DateTime expires_at { get; set; }
        public bool is_used { get; set; }
        public DateTime created_at { get; set; }
    }
}

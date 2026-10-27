using PhotographyPortfolioAPI.Core.Enums.Auth;

namespace PhotographyPortfolioAPI.Contracts.Models.Auth
{
    public class User
    {
        public Guid user_id { get; set; }
        public string user_name { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string password_hash { get; set; }
        public string password_salt { get; set; }
        public bool is_active { get; set; }
        public UserType user_type { get; set; }
        public Role? role { get; set; }
        public DateTime created_at { get; set; }
        public string profile_url { get; set; }
        public string online_status { get; set; }
        public string status_message { get; set; }
        public string designation_name { get; set; }
        public string department_name { get; set; }
        public bool is_email_verified { get; set; }
        public bool is_mobile_verified { get; set; }
        public string profile_img_path { get; set; }
        public string profile_banner_path { get; set; }
    }
}

using PhotographyPortfolioAPI.Contracts.Models.Auth;

namespace PhotographyPortfolioAPI.Contracts.DTOs
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public UserDto User { get; set; }
    }

    public class UserDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserType { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public bool IsActive { get; set; }
        public List<UserPermissionsData> PermissionList { get; set; }
        public Role UserRole { get; set; }
        public string? profile_img_path { get; set; }
        public string? profile_banner_path { get; set; }
        public string online_status { get; set; }
        public string status_message { get; set; }
        public bool is_email_verified { get; set; }
        public bool is_mobile_verified { get; set; }
        public bool is_active { get; set; }
    }
}

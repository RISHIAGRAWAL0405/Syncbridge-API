using Microsoft.AspNetCore.Http;

namespace PhotographyPortfolioAPI.Contracts.DTOs.MyProfile
{
    public class ChangePasswordRequest
    {
        public string? old_password { get; set; }
        public string? new_password { get; set; }
    }

    public class UpdateProfileRequest
    {
        public string? email { get; set; }
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public string? mobile { get; set; }
        public string? profile_url { get; set; }
    }

    public class ProfileImageRequest
    {
        public IFormFile? ProfileImage { get; set; }
    }

    public class ProfilebannerRequest
    {
        public IFormFile? Profilebanner { get; set; }
    }

    public class UpdateOnlineStatus
    {
        public string? online_status { get; set; }
    }

    public class UpdateStatusMessage
    {
        public string? status_message { get; set; }
    }
}

using PhotographyPortfolioAPI.Core.Enums.Auth;

namespace PhotographyPortfolioAPI.Contracts.Models.Auth
{
    public class LoginRequest
    {
        public string user_name { get; set; }
        public string password { get; set; }
    }

    public class RefreshTokenRequest
    {
        public string refresh_token { get; set; }
    }

    public class RegisterRequest
    {
        public string user_name { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string password { get; set; }
        public UserType user_type { get; set; }
    }

    public class UserCreateRequest
    {
        public string user_name { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string? mobile { get; set; }
        public string user_type { get; set; }
        public string password { get; set; }
        public string? designation_name { get; set; }
        public string? department_name { get; set; }
        public Guid? role_id { get; set; }
    }

    public class UserUpdateRequest
    {
        public string email { get; set; }
        public string mobile { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string user_type { get; set; }
        public Guid? role_id { get; set; }
        public string? designation_name { get; set; }
        public string? department_name { get; set; }
    }

    public class RoleCreateRequest
    {
        public string role_code { get; set; }
        public string role_name { get; set; }
    }

    public class RoleUpdateRequest
    {
        public Guid role_id { get; set; }
        public string role_name { get; set; }
    }

    public class PermissionCreateRequest
    {
        public string permission_code { get; set; }
        public string permission_name { get; set; }
    }

    public class SendEmailVerificationRequest
    {
        public Guid user_id { get; set; }
    }

    public class VerifyEmailOtpRequest
    {
        public Guid user_id { get; set; }
        public string otp_code { get; set; }
    }

    public class ForgotPasswordRequest
    {
        public string user_name { get; set; }
    }

    public class ResetPasswordRequest
    {
        public Guid user_id { get; set; }
        public string otp_code { get; set; }
        public string new_password { get; set; }
    }

    public class UpdateRolePermissionsRequestData
    {
        public List<UpdateRolePermissionsDetails> PermissionsList { get; set; }
    }

    public class UpdateRolePermissionsDetails
    {
        public Guid? role_id { get; set; }
        public Guid permission_id { get; set; }
        public bool can_view { get; set; }
        public bool can_add { get; set; }
        public bool can_update { get; set; }
        public bool can_delete { get; set; }
        public bool can_export { get; set; }
    }
}

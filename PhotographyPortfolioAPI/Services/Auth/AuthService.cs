using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs;
using PhotographyPortfolioAPI.Contracts.Models.Auth;
using PhotographyPortfolioAPI.Core.Enums.Auth;
using PhotographyPortfolioAPI.Helpers;
using PhotographyPortfolioAPI.Infrastructure.Helpers.Auth;
using PhotographyPortfolioAPI.Infrastructure.Interfaces.Auth;
using PhotographyPortfolioAPI.Interfaces.Auth;

namespace PhotographyPortfolioAPI.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly ITokenRepository _tokenRepo;
        private readonly IJwtService _jwt;

        public AuthService(IUserRepository userRepo, ITokenRepository tokenRepo, IJwtService jwt)
        {
            _userRepo = userRepo;
            _tokenRepo = tokenRepo;
            _jwt = jwt;
        }

        public async Task<AuthResponseDto> LoginAsync(string userName, string password)
        {
            var user = await _userRepo.GetByUserNameAsync(userName);
            if (user == null) throw new UnauthorizedAccessException("Invalid username or password");
            if (!user.is_active) throw new UnauthorizedAccessException("User account is inactive");
            if (!PasswordHelper.VerifyPassword(password, user.password_hash, user.password_salt))
                throw new UnauthorizedAccessException("Invalid username or password");

            var permissions = await _userRepo.GetPermissions(user.user_id);
            var role = await _userRepo.GetUserRoleAsync(user.user_id);
            if (role?.is_assignable == false)
                permissions = await _userRepo.GetAllPermissionForSuperAdmin();

            var accessToken = _jwt.GenerateAccessTokenAsync(user, permissions);
            var refreshToken = _jwt.GenerateRefreshTokenAsync();

            await _tokenRepo.SaveRefreshTokenAsync(new RefreshToken
            {
                refresh_token_id = Guid.NewGuid(), user_id = user.user_id,
                token = refreshToken, expires_at = DateTime.UtcNow.AddDays(7), is_revoked = false
            });

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                User = new UserDto
                {
                    UserId = user.user_id, UserName = user.user_name,
                    FirstName = user.first_name, LastName = user.last_name,
                    IsActive = user.is_active, Email = user.email, Mobile = user.mobile,
                    UserType = Convert.ToString(user.user_type),
                    PermissionList = permissions, UserRole = role,
                    online_status = user.online_status, status_message = user.status_message,
                    profile_img_path = user.profile_img_path,
                    profile_banner_path = user.profile_banner_path
                }
            };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var token = await _tokenRepo.GetRefreshTokenAsync(refreshToken);
            if (token == null || token.is_revoked) throw new UnauthorizedAccessException("Invalid or revoked refresh token");
            if (token.expires_at < DateTime.UtcNow) throw new UnauthorizedAccessException("Refresh token has expired");

            var user = await _userRepo.GetByIdAsync(token.user_id);
            if (user == null || !user.is_active) throw new UnauthorizedAccessException("User not found or inactive");

            var permissions = await _userRepo.GetPermissions(user.user_id);
            var newAccessToken = _jwt.GenerateAccessTokenAsync(user, permissions);
            var newRefreshToken = _jwt.GenerateRefreshTokenAsync();

            await _tokenRepo.Revoke(refreshToken);
            await _tokenRepo.SaveRefreshTokenAsync(new RefreshToken
            {
                refresh_token_id = Guid.NewGuid(), user_id = user.user_id,
                token = newRefreshToken, expires_at = DateTime.UtcNow.AddDays(7), is_revoked = false
            });

            return new AuthResponseDto
            {
                AccessToken = newAccessToken, RefreshToken = newRefreshToken,
                User = new UserDto { UserId = user.user_id, UserName = user.user_name, IsActive = user.is_active }
            };
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var token = await _tokenRepo.GetRefreshTokenAsync(refreshToken);
            if (token == null) throw new InvalidOperationException("Refresh token not found");
            await _tokenRepo.Revoke(refreshToken);
        }

        public async Task<ApiResponseData<object>> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var user = await _userRepo.GetByUserNameAsync(request.user_name);
            if (user == null) throw new KeyNotFoundException("No account found with this username");
            if (!user.is_active) throw new InvalidOperationException("User account is inactive");

            var otp = UserAuthenticationHelper.GenerateOtp();
            var expiryMinutes = UserAuthenticationHelper.GetOtpExpiryInMinutes(UserAuthOtpType.RESET_PASSWORD);

            await _userRepo.CreateOtpAsync(new UserOtp
            {
                user_id = user.user_id, otp_code = otp,
                otp_type = UserAuthOtpType.RESET_PASSWORD.ToString(),
                expires_at = DateTime.UtcNow.AddMinutes(expiryMinutes)
            });

            // TODO: Send OTP via email
            return ApiResponseData<object>.Success(new { user_id = user.user_id }, "Password reset OTP sent to your email");
        }

        public async Task<ApiResponseData<object>> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userRepo.GetByIdAsync(request.user_id);
            if (user == null) throw new KeyNotFoundException("User not found");

            var otp = await _userRepo.GetActiveOtpAsync(request.user_id, request.otp_code, UserAuthOtpType.RESET_PASSWORD.ToString());
            if (otp == null) throw new InvalidOperationException("Invalid or expired OTP");

            PasswordHelper.CreatePasswordHash(request.new_password, out var hash, out var salt);
            await _userRepo.MarkOtpUsedAsync(otp.otp_id);
            await _userRepo.UpdatePasswordAsync(request.user_id, hash, salt);

            return ApiResponseData<object>.Success(null, "Password reset successfully");
        }

        public async Task<ApiResponseData<object>> SendEmailVerificationOtpAsync(Guid userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");
            if (user.is_email_verified) throw new InvalidOperationException("Email is already verified");

            var otp = UserAuthenticationHelper.GenerateOtp();
            var expiryMinutes = UserAuthenticationHelper.GetOtpExpiryInMinutes(UserAuthOtpType.EMAIL_VERIFICATION);

            await _userRepo.CreateOtpAsync(new UserOtp
            {
                user_id = userId, otp_code = otp,
                otp_type = UserAuthOtpType.EMAIL_VERIFICATION.ToString(),
                expires_at = DateTime.UtcNow.AddMinutes(expiryMinutes)
            });

            // TODO: Send OTP via email
            return ApiResponseData<object>.Success(null, "Verification OTP sent to your email");
        }

        public async Task<ApiResponseData<object>> VerifyEmailOtpAsync(VerifyEmailOtpRequest request)
        {
            var user = await _userRepo.GetByIdAsync(request.user_id);
            if (user == null) throw new KeyNotFoundException("User not found");

            var otp = await _userRepo.GetActiveOtpAsync(request.user_id, request.otp_code, UserAuthOtpType.EMAIL_VERIFICATION.ToString());
            if (otp == null) throw new InvalidOperationException("Invalid or expired OTP");

            await _userRepo.MarkOtpUsedAsync(otp.otp_id);
            await _userRepo.UpdateEmailVerifiedAsync(request.user_id);

            return ApiResponseData<object>.Success(null, "Email verified successfully");
        }
    }
}

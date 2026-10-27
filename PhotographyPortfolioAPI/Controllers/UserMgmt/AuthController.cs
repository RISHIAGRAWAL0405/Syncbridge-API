using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs;
using PhotographyPortfolioAPI.Contracts.Models.Auth;
using PhotographyPortfolioAPI.Interfaces.Auth;
using Microsoft.AspNetCore.Mvc;

namespace PhotographyPortfolioAPI.Controllers.UserMgmt
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("login")]
        public async Task<ApiResponseData<AuthResponseDto>> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.user_name)) return ApiResponseData<AuthResponseDto>.Error("Username is required", 400);
                if (string.IsNullOrWhiteSpace(request.password)) return ApiResponseData<AuthResponseDto>.Error("Password is required", 400);
                var result = await _authService.LoginAsync(request.user_name, request.password);
                return ApiResponseData<AuthResponseDto>.Success(result, "Login successful");
            }
            catch (UnauthorizedAccessException ex) { return ApiResponseData<AuthResponseDto>.Error(ex.Message, 401); }
            catch (Exception ex) { return ApiResponseData<AuthResponseDto>.Error("An error occurred during login", 500); }
        }

        [HttpPost("refresh")]
        public async Task<ApiResponseData<AuthResponseDto>> Refresh([FromBody] RefreshTokenRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.refresh_token)) return ApiResponseData<AuthResponseDto>.Error("Refresh token is required", 400);
                var result = await _authService.RefreshTokenAsync(request.refresh_token);
                return ApiResponseData<AuthResponseDto>.Success(result, "Token refreshed successfully");
            }
            catch (UnauthorizedAccessException ex) { return ApiResponseData<AuthResponseDto>.Error(ex.Message, 401); }
            catch (Exception) { return ApiResponseData<AuthResponseDto>.Error("An error occurred during token refresh", 500); }
        }

        [HttpPost("logout")]
        public async Task<ApiResponseData<object>> Logout([FromBody] RefreshTokenRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.refresh_token)) return ApiResponseData<object>.Error("Refresh token is required", 400);
                await _authService.LogoutAsync(request.refresh_token);
                return ApiResponseData<object>.Success(null, "Logged out successfully");
            }
            catch (InvalidOperationException ex) { return ApiResponseData<object>.Error(ex.Message, 400); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred during logout", 500); }
        }

        [HttpPost("forgot-password")]
        public async Task<ApiResponseData<object>> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.user_name)) return ApiResponseData<object>.Error("Username is required", 400);
                return await _authService.ForgotPasswordAsync(request);
            }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (InvalidOperationException ex) { return ApiResponseData<object>.Error(ex.Message, 400); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred", 500); }
        }

        [HttpPost("reset-password")]
        public async Task<ApiResponseData<object>> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                if (request.user_id == Guid.Empty) return ApiResponseData<object>.Error("User ID is required", 400);
                if (string.IsNullOrWhiteSpace(request.otp_code)) return ApiResponseData<object>.Error("OTP code is required", 400);
                if (string.IsNullOrWhiteSpace(request.new_password)) return ApiResponseData<object>.Error("New password is required", 400);
                return await _authService.ResetPasswordAsync(request);
            }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (InvalidOperationException ex) { return ApiResponseData<object>.Error(ex.Message, 400); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred during password reset", 500); }
        }
    }
}

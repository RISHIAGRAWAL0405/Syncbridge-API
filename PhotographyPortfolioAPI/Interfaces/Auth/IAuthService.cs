using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.Models.Auth;
using PhotographyPortfolioAPI.Contracts.DTOs;

namespace PhotographyPortfolioAPI.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(string userName, string password);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(string refreshToken);
        Task<ApiResponseData<object>> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<ApiResponseData<object>> ResetPasswordAsync(ResetPasswordRequest request);
        Task<ApiResponseData<object>> SendEmailVerificationOtpAsync(Guid userId);
        Task<ApiResponseData<object>> VerifyEmailOtpAsync(VerifyEmailOtpRequest request);
    }
}

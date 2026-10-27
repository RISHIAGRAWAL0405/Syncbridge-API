using PhotographyPortfolioAPI.Contracts.DTOs.Auth;
using PhotographyPortfolioAPI.Contracts.DTOs.MyProfile;
using PhotographyPortfolioAPI.Contracts.Models.Auth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace PhotographyPortfolioAPI.Infrastructure.Interfaces.Auth
{
    public interface IUserRepository
    {
        Task CreateAsync(User user);
        Task UpdateAsync(User user);
        Task<User?> GetByUserNameAsync(string userName);
        Task<User?> GetByIdAsync(Guid userId);
        Task<User?> GetByEmailAsync(string email);
        Task<UserMasterDto?> GetUserMasterDtoByUserIdAsync(Guid userId);
        Task UpdateUserRoleAsync(Guid userId, Guid roleId);
        Task<Role?> GetUserRoleAsync(Guid userId);
        Task<List<UserPermissionsData>> GetPermissions(Guid userId);
        Task<List<UserPermissionsData>> GetAllPermissionForSuperAdmin();
        Task<List<UserMasterWithRoleDto>> GetAllUsersAsync();
        Task<List<UserMasterDto>> GetAllActiveUsersAsync();
        Task<List<UserMasterDto>> GetAllActiveUsersByUserTypeAsync(string user_type);
        Task ActiveInactiveAsync(ActiveInactiveRequest request);
        Task<List<UserMasterDto>> GetUsersByIdsAsync(IEnumerable<Guid> userIds);

        Task<UserMasterDto?> GetProfileByIdAsync(Guid userId);
        Task<string> ChangeProfileImageAsync(IFormFile file, Guid userId, IWebHostEnvironment env);
        Task<string> ChangeProfilebannerAsync(IFormFile file, Guid userId, IWebHostEnvironment env);
        Task<UserMasterDto?> UpdateProfileAsync(UserMasterDto user);
        Task ChangePasswordAsync(string hash, string salt, Guid userId);
        Task UpdateOnlineStatusAsync(UpdateOnlineStatus request, Guid userId);
        Task UpdateStatusMessageAsync(UpdateStatusMessage request, Guid userId);

        Task CreateOtpAsync(UserOtp otp);
        Task<UserOtp?> GetActiveOtpAsync(Guid userId, string otpCode, string otpType);
        Task MarkOtpUsedAsync(long otpId);
        Task UpdateEmailVerifiedAsync(Guid userId);
        Task UpdatePasswordAsync(Guid userId, string passwordHash, string passwordSalt);
        Task<List<UserMasterDto>> GetPendingClientsAsync();
    }
}

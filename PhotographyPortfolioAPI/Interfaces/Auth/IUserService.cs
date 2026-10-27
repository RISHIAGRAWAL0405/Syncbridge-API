using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs.Auth;
using PhotographyPortfolioAPI.Contracts.DTOs.MyProfile;
using PhotographyPortfolioAPI.Contracts.Models.Auth;
using Microsoft.AspNetCore.Hosting;

namespace PhotographyPortfolioAPI.Interfaces.Auth
{
    public interface IUserService
    {
        Task<Guid> CreateUserAsync(UserCreateRequest request);
        Task<Guid> UpdateUserAsync(Guid userId, UserUpdateRequest request);
        Task UpdateUserRoleAsync(Guid userId, Guid roleId);
        Task<Role> GetUserRoleAsync(Guid userId);
        Task<List<UserPermissionsData>> GetUserPermissionsAsync(Guid userId);
        Task<User> GetUserByIdAsync(Guid userId);
        Task<bool> CheckUsernameExistsAsync(string username);
        Task<List<UserMasterWithRoleDto>> GetAllUsersAsync();
        Task<List<UserMasterDto>> GetAllActiveUsersAsync();
        Task<List<UserMasterDto>> GetAllActiveUsersByUserTypeAsync(string user_type);
        Task<Role?> GetUserAssignedRole(Guid userId);
        Task<UserMasterDto?> GetUserMasterDtoByIdAsync(Guid userId);
        Task<ApiResponseData<object>> ActiveInactive(ActiveInactiveRequest request);

        Task<UserMasterDto?> GetMyProfile(Guid userId);
        Task<ApiResponseData<object>> ChangeProfileImage(ProfileImageRequest request, Guid userId, IWebHostEnvironment env);
        Task<ApiResponseData<object>> ChangeProfilebanner(ProfilebannerRequest request, Guid userId, IWebHostEnvironment env);
        Task<UserMasterDto> UpdateProfileAsync(Guid userId, UpdateProfileRequest request);
        Task<ApiResponseData<object>> ChangePassword(ChangePasswordRequest request, Guid userId);
        Task<ApiResponseData<object>> UpdateOnlineStatusAsync(UpdateOnlineStatus request, Guid userId);
        Task<ApiResponseData<object>> UpdateStatusMessageAsync(UpdateStatusMessage request, Guid userId);
        Task<List<UserMasterDto>> GetPendingClientsAsync();
    }
}

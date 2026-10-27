using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs.Auth;
using PhotographyPortfolioAPI.Contracts.DTOs.MyProfile;
using PhotographyPortfolioAPI.Contracts.Models.Auth;
using PhotographyPortfolioAPI.Core.Enums.Auth;
using PhotographyPortfolioAPI.Helpers;
using PhotographyPortfolioAPI.Infrastructure.Interfaces.Auth;
using PhotographyPortfolioAPI.Interfaces.Auth;
using Microsoft.AspNetCore.Hosting;

namespace PhotographyPortfolioAPI.Services.Auth
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<Guid> CreateUserAsync(UserCreateRequest request)
        {
            var existingUser = await _userRepo.GetByUserNameAsync(request.user_name);
            if (existingUser != null) throw new InvalidOperationException("Username already exists");

            PasswordHelper.CreatePasswordHash(request.password, out var hash, out var salt);
            var userId = Guid.NewGuid();
            var user = new User
            {
                user_id = userId, user_name = request.user_name, email = request.email,
                mobile = request.mobile, first_name = request.first_name, last_name = request.last_name,
                password_hash = hash, password_salt = salt, is_active = true,
                user_type = Enum.Parse<UserType>(request.user_type),
                designation_name = request.designation_name, department_name = request.department_name
            };

            await _userRepo.CreateAsync(user);
            if (request.role_id.HasValue)
                await _userRepo.UpdateUserRoleAsync(userId, request.role_id.Value);

            return userId;
        }

        public async Task<Guid> UpdateUserAsync(Guid userId, UserUpdateRequest request)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");

            if (!string.IsNullOrWhiteSpace(request.email)) user.email = request.email;
            if (!string.IsNullOrWhiteSpace(request.first_name)) user.first_name = request.first_name;
            if (!string.IsNullOrWhiteSpace(request.last_name)) user.last_name = request.last_name;
            if (!string.IsNullOrWhiteSpace(request.mobile)) user.mobile = request.mobile;
            if (!string.IsNullOrWhiteSpace(request.user_type)) user.user_type = Enum.Parse<UserType>(request.user_type);
            if (!string.IsNullOrWhiteSpace(request.designation_name)) user.designation_name = request.designation_name;
            if (!string.IsNullOrWhiteSpace(request.department_name)) user.department_name = request.department_name;

            await _userRepo.UpdateAsync(user);
            if (request.role_id.HasValue)
                await _userRepo.UpdateUserRoleAsync(userId, request.role_id.Value);

            return userId;
        }

        public async Task UpdateUserRoleAsync(Guid userId, Guid roleId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");
            await _userRepo.UpdateUserRoleAsync(userId, roleId);
        }

        public async Task<Role> GetUserRoleAsync(Guid userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");
            var role = await _userRepo.GetUserRoleAsync(userId);
            if (role == null) throw new KeyNotFoundException("User has no role assigned");
            return role;
        }

        public async Task<List<UserPermissionsData>> GetUserPermissionsAsync(Guid userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");
            return await _userRepo.GetPermissions(userId);
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }

        public async Task<bool> CheckUsernameExistsAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return false;
            return await _userRepo.GetByUserNameAsync(username.Trim()) != null;
        }

        public async Task<List<UserMasterWithRoleDto>> GetAllUsersAsync()
            => await _userRepo.GetAllUsersAsync();

        public async Task<List<UserMasterDto>> GetAllActiveUsersAsync()
            => await _userRepo.GetAllActiveUsersAsync();

        public async Task<List<UserMasterDto>> GetAllActiveUsersByUserTypeAsync(string user_type)
            => await _userRepo.GetAllActiveUsersByUserTypeAsync(user_type);

        public async Task<Role?> GetUserAssignedRole(Guid userId)
            => await _userRepo.GetUserRoleAsync(userId);

        public async Task<UserMasterDto?> GetUserMasterDtoByIdAsync(Guid userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) return null;
            return new UserMasterDto
            {
                user_id = user.user_id, user_name = user.user_name, email = user.email,
                mobile = user.mobile, first_name = user.first_name, last_name = user.last_name,
                full_name = $"{user.first_name} {user.last_name}"
            };
        }

        public async Task<ApiResponseData<object>> ActiveInactive(ActiveInactiveRequest request)
        {
            if (request.user_id == Guid.Empty) throw new ArgumentException("user id is required.");
            var user = await _userRepo.GetByIdAsync(request.user_id);
            if (user == null) throw new ArgumentException("User not found");
            await _userRepo.ActiveInactiveAsync(request);
            return ApiResponseData<object>.Success(new { id = request.user_id }, "Active/Inactive update successfully");
        }

        public async Task<UserMasterDto?> GetMyProfile(Guid userId)
            => await _userRepo.GetProfileByIdAsync(userId);

        public async Task<ApiResponseData<object>> ChangeProfileImage(ProfileImageRequest request, Guid userId, IWebHostEnvironment env)
        {
            var user = await _userRepo.GetProfileByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");
            var imagePath = await _userRepo.ChangeProfileImageAsync(request.ProfileImage, userId, env);
            return ApiResponseData<object>.Success(new { ProfileUrl = imagePath }, "Profile image updated successfully");
        }

        public async Task<ApiResponseData<object>> ChangeProfilebanner(ProfilebannerRequest request, Guid userId, IWebHostEnvironment env)
        {
            var user = await _userRepo.GetProfileByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");
            var imagePath = await _userRepo.ChangeProfilebannerAsync(request.Profilebanner, userId, env);
            return ApiResponseData<object>.Success(new { ProfileUrl = imagePath }, "Profile banner updated successfully");
        }

        public async Task<UserMasterDto> UpdateProfileAsync(Guid userId, UpdateProfileRequest request)
        {
            var user = await _userRepo.GetProfileByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("Profile not found");

            if (!string.IsNullOrWhiteSpace(request.email)) user.email = request.email;
            if (!string.IsNullOrWhiteSpace(request.first_name)) user.first_name = request.first_name;
            if (!string.IsNullOrWhiteSpace(request.last_name)) user.last_name = request.last_name;
            if (!string.IsNullOrWhiteSpace(request.mobile)) user.mobile = request.mobile;
            if (!string.IsNullOrWhiteSpace(request.profile_url)) user.profile_url = request.profile_url;
            user.full_name = $"{user.first_name} {user.last_name}".Trim();

            return await _userRepo.UpdateProfileAsync(user);
        }

        public async Task<ApiResponseData<object>> ChangePassword(ChangePasswordRequest request, Guid userId)
        {
            if (request.old_password == null) throw new ArgumentException("Old Password is required.");
            if (request.new_password == null) throw new ArgumentException("New Password is required.");

            var user = await _userRepo.GetProfileByIdAsync(userId);
            if (user == null) throw new ArgumentException("User not found");
            if (!PasswordHelper.VerifyPassword(request.old_password, user.password_hash, user.password_salt))
                throw new ArgumentException("Old Password Wrong");

            PasswordHelper.CreatePasswordHash(request.new_password, out var hash, out var salt);
            await _userRepo.ChangePasswordAsync(hash, salt, userId);
            return ApiResponseData<object>.Success(new { id = userId }, "Password changed successfully");
        }

        public async Task<ApiResponseData<object>> UpdateOnlineStatusAsync(UpdateOnlineStatus request, Guid userId)
        {
            if (request.online_status == null) throw new ArgumentException("online_status is required.");
            var user = await _userRepo.GetProfileByIdAsync(userId);
            if (user == null) throw new ArgumentException("User not found");
            await _userRepo.UpdateOnlineStatusAsync(request, userId);
            return ApiResponseData<object>.Success(new { id = userId }, "Online status updated successfully");
        }

        public async Task<ApiResponseData<object>> UpdateStatusMessageAsync(UpdateStatusMessage request, Guid userId)
        {
            if (request.status_message == null) throw new ArgumentException("status_message is required.");
            var user = await _userRepo.GetProfileByIdAsync(userId);
            if (user == null) throw new ArgumentException("User not found");
            await _userRepo.UpdateStatusMessageAsync(request, userId);
            return ApiResponseData<object>.Success(new { id = userId }, "Status message updated successfully");
        }

        public async Task<List<UserMasterDto>> GetPendingClientsAsync()
            => await _userRepo.GetPendingClientsAsync();
    }
}

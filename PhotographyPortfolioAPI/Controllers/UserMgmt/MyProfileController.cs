using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs.Auth;
using PhotographyPortfolioAPI.Contracts.DTOs.MyProfile;
using PhotographyPortfolioAPI.Extensions;
using PhotographyPortfolioAPI.Interfaces.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PhotographyPortfolioAPI.Controllers.UserMgmt
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MyProfileController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IUserService _userService;

        public MyProfileController(IWebHostEnvironment env, IUserService userService)
        {
            _env = env;
            _userService = userService;
        }

        [HttpGet("getMyProfile")]
        public async Task<ApiResponseData<UserMasterDto>> GetMyProfile()
        {
            try
            {
                Guid userId = User.GetUserId();
                var result = await _userService.GetMyProfile(userId);
                return ApiResponseData<UserMasterDto>.Success(result, "User retrieved successfully");
            }
            catch (KeyNotFoundException ex) { return ApiResponseData<UserMasterDto>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<UserMasterDto>.Error("An error occurred", 500); }
        }

        [HttpPost("updateProfileImage")]
        [Consumes("multipart/form-data")]
        public async Task<ApiResponseData<object>> UpdateProfileImage([FromForm] ProfileImageRequest request)
        {
            try
            {
                Guid userId = User.GetUserId();
                return await _userService.ChangeProfileImage(request, userId, _env);
            }
            catch (ArgumentException ex) { return ApiResponseData<object>.Error(ex.Message, 400); }
            catch (Exception) { return ApiResponseData<object>.Error("Error while changing profile image", 500); }
        }

        [HttpPost("updateProfilebanner")]
        [Consumes("multipart/form-data")]
        public async Task<ApiResponseData<object>> UpdateProfilebanner([FromForm] ProfilebannerRequest request)
        {
            try
            {
                Guid userId = User.GetUserId();
                return await _userService.ChangeProfilebanner(request, userId, _env);
            }
            catch (ArgumentException ex) { return ApiResponseData<object>.Error(ex.Message, 400); }
            catch (Exception) { return ApiResponseData<object>.Error("Error while changing profile banner", 500); }
        }

        [HttpPut("updateProfiledetails")]
        public async Task<ApiResponseData<object>> UpdateProfiledetails([FromBody] UpdateProfileRequest request)
        {
            try
            {
                Guid userId = User.GetUserId();
                var result = await _userService.UpdateProfileAsync(userId, request);
                return ApiResponseData<object>.Success(result, "Profile updated successfully", userId);
            }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred while updating profile", 500); }
        }

        [HttpPost("changePassword")]
        public async Task<ApiResponseData<object>> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                Guid userId = User.GetUserId();
                return await _userService.ChangePassword(request, userId);
            }
            catch (ArgumentException ex) { return ApiResponseData<object>.Error(ex.Message, 400); }
            catch (Exception) { return ApiResponseData<object>.Error("Error while changing password", 500); }
        }

        [HttpPut("updateOnlineStatus")]
        public async Task<ApiResponseData<object>> UpdateOnlineStatus([FromBody] UpdateOnlineStatus request)
        {
            try
            {
                Guid userId = User.GetUserId();
                return await _userService.UpdateOnlineStatusAsync(request, userId);
            }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred", 500); }
        }

        [HttpPut("updateStatusMessage")]
        public async Task<ApiResponseData<object>> UpdateStatusMessage([FromBody] UpdateStatusMessage request)
        {
            try
            {
                Guid userId = User.GetUserId();
                return await _userService.UpdateStatusMessageAsync(request, userId);
            }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred", 500); }
        }
    }
}

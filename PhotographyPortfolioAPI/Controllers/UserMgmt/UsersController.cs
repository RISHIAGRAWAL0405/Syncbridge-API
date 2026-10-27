using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs.Auth;
using PhotographyPortfolioAPI.Contracts.Models.Auth;
using PhotographyPortfolioAPI.Extensions;
using PhotographyPortfolioAPI.Interfaces.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PhotographyPortfolioAPI.Controllers.UserMgmt
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public UsersController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost]
        public async Task<ApiResponseData<object>> Create([FromBody] UserCreateRequest request)
        {
            try
            {
                var userId = await _userService.CreateUserAsync(request);
                return ApiResponseData<object>.Success(null, "User created successfully", userId);
            }
            catch (InvalidOperationException ex) { return ApiResponseData<object>.Error(ex.Message, 400); }
            catch (Exception ex) { 
                return ApiResponseData<object>.Error("An error occurred while creating user", 500); 
                }
        }

        [HttpPut("{user_id}")]
        public async Task<ApiResponseData<object>> Update(Guid user_id, [FromBody] UserUpdateRequest request)
        {
            try
            {
                await _userService.UpdateUserAsync(user_id, request);
                return ApiResponseData<object>.Success(null, "User updated successfully", user_id);
            }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred while updating user", 500); }
        }

        [HttpGet("{user_id}")]
        public async Task<ApiResponseData<UserMasterDto>> GetById(Guid user_id)
        {
            try
            {
                var user = await _userService.GetMyProfile(user_id);
                user.role = await _userService.GetUserAssignedRole(user_id);
                return ApiResponseData<UserMasterDto>.Success(user, "User retrieved successfully");
            }
            catch (KeyNotFoundException ex) { return ApiResponseData<UserMasterDto>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<UserMasterDto>.Error("An error occurred while retrieving user", 500); }
        }

        [HttpGet("GetAllUsersAsync")]
        public async Task<ApiResponseData<List<UserMasterWithRoleDto>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return ApiResponseData<List<UserMasterWithRoleDto>>.Success(users, "Users retrieved successfully");
            }
            catch (Exception) { return ApiResponseData<List<UserMasterWithRoleDto>>.Error("An error occurred while retrieving users", 500); }
        }

        [HttpGet("GetAllActiveUsersAsync")]
        public async Task<ApiResponseData<List<UserMasterDto>>> GetAllActiveUsersAsync()
        {
            try
            {
                var users = await _userService.GetAllActiveUsersAsync();
                return ApiResponseData<List<UserMasterDto>>.Success(users, "Users retrieved successfully");
            }
            catch (Exception) { return ApiResponseData<List<UserMasterDto>>.Error("An error occurred", 500); }
        }

        [HttpGet("GetAllActiveUsersByUserType")]
        public async Task<ApiResponseData<List<UserMasterDto>>> GetAllActiveUsersByUserType(string user_type)
        {
            try
            {
                var users = await _userService.GetAllActiveUsersByUserTypeAsync(user_type);
                return ApiResponseData<List<UserMasterDto>>.Success(users, "Users retrieved successfully");
            }
            catch (Exception) { return ApiResponseData<List<UserMasterDto>>.Error("An error occurred", 500); }
        }

        [HttpGet("pending-clients")]
        public async Task<ApiResponseData<List<UserMasterDto>>> GetPendingClients()
        {
            try
            {
                var users = await _userService.GetPendingClientsAsync();
                return ApiResponseData<List<UserMasterDto>>.Success(users, "Pending clients retrieved successfully");
            }
            catch (Exception) { return ApiResponseData<List<UserMasterDto>>.Error("An error occurred", 500); }
        }

        [HttpPost("check-username")]
        public async Task<ApiResponseData<CheckUsernameResponse>> CheckUsernameExists([FromBody] CheckUsernameRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request?.username)) return ApiResponseData<CheckUsernameResponse>.Error("Username is required", 400);
                var exists = await _userService.CheckUsernameExistsAsync(request.username);
                return ApiResponseData<CheckUsernameResponse>.Success(new CheckUsernameResponse
                {
                    exists = exists,
                    message = exists ? "Username already exists" : "Username is available"
                }, "Username check completed");
            }
            catch (Exception) { return ApiResponseData<CheckUsernameResponse>.Error("An error occurred", 500); }
        }

        [HttpPost("activeInactive")]
        public async Task<ApiResponseData<object>> ActiveInactive([FromBody] ActiveInactiveRequest request)
        {
            try { return await _userService.ActiveInactive(request); }
            catch (ArgumentException ex) { return ApiResponseData<object>.Error(ex.Message, 400); }
            catch (Exception) { return ApiResponseData<object>.Error("Error while Active/Inactive User", 500); }
        }

        [HttpPost("send-email-verification")]
        public async Task<ApiResponseData<object>> SendEmailVerification([FromBody] SendEmailVerificationRequest request)
        {
            try
            {
                if (request.user_id == Guid.Empty) return ApiResponseData<object>.Error("User ID is required", 400);
                return await _authService.SendEmailVerificationOtpAsync(request.user_id);
            }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (InvalidOperationException ex) { return ApiResponseData<object>.Error(ex.Message, 400); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred", 500); }
        }

        [HttpPost("verify-email")]
        public async Task<ApiResponseData<object>> VerifyEmail([FromBody] VerifyEmailOtpRequest request)
        {
            try
            {
                if (request.user_id == Guid.Empty) return ApiResponseData<object>.Error("User ID is required", 400);
                if (string.IsNullOrWhiteSpace(request.otp_code)) return ApiResponseData<object>.Error("OTP code is required", 400);
                return await _authService.VerifyEmailOtpAsync(request);
            }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (InvalidOperationException ex) { return ApiResponseData<object>.Error(ex.Message, 400); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred", 500); }
        }
    }
}

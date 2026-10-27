using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.Models.Auth;
using PhotographyPortfolioAPI.Interfaces.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PhotographyPortfolioAPI.Controllers.UserMgmt
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService) => _roleService = roleService;

        [HttpPost]
        public async Task<ApiResponseData<object>> Create([FromBody] RoleCreateRequest request)
        {
            try
            {
                var roleId = await _roleService.CreateRoleAsync(request);
                return ApiResponseData<object>.Success(null, "Role created successfully", roleId);
            }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred while creating role", 500); }
        }

        [HttpPut("{role_id}")]
        public async Task<ApiResponseData<object>> Update(Guid role_id, [FromBody] RoleUpdateRequest request)
        {
            try
            {
                await _roleService.UpdateRoleAsync(role_id, request);
                return ApiResponseData<object>.Success(null, "Role updated successfully", role_id);
            }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred while updating role", 500); }
        }

        [HttpGet("{role_id}")]
        public async Task<ApiResponseData<Role>> GetById(Guid role_id)
        {
            try
            {
                var role = await _roleService.GetRoleByIdAsync(role_id);
                return ApiResponseData<Role>.Success(role, "Role retrieved successfully");
            }
            catch (KeyNotFoundException ex) { return ApiResponseData<Role>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<Role>.Error("An error occurred", 500); }
        }

        [HttpGet]
        public async Task<ApiResponseData<List<Role>>> GetAll()
        {
            try
            {
                var roles = await _roleService.GetAllRolesAsync();
                return ApiResponseData<List<Role>>.Success(roles, "Roles retrieved successfully");
            }
            catch (Exception) { return ApiResponseData<List<Role>>.Error("An error occurred", 500); }
        }

        [HttpPut("{role_id}/permissions")]
        public async Task<ApiResponseData<object>> UpdatePermissions(Guid role_id, [FromBody] UpdateRolePermissionsRequestData request)
        {
            try
            {
                await _roleService.UpdateRolePermissionsAsync(role_id, request);
                return ApiResponseData<object>.Success(null, "Permissions updated successfully");
            }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred while updating permissions", 500); }
        }

        [HttpGet("{role_id}/permissions")]
        public async Task<ApiResponseData<List<Permission>>> GetRolePermissions(Guid role_id)
        {
            try
            {
                var permissions = await _roleService.GetRolePermissionsAsync(role_id);
                return ApiResponseData<List<Permission>>.Success(permissions, "Role permissions retrieved successfully");
            }
            catch (Exception) { return ApiResponseData<List<Permission>>.Error("An error occurred", 500); }
        }
    }
}

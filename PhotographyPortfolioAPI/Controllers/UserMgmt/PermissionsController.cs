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
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionsController(IPermissionService permissionService) => _permissionService = permissionService;

        [HttpPost]
        public async Task<ApiResponseData<object>> Create([FromBody] PermissionCreateRequest request)
        {
            try
            {
                var permissionId = await _permissionService.CreatePermissionAsync(request);
                return ApiResponseData<object>.Success(null, "Permission created successfully", permissionId);
            }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred while creating permission", 500); }
        }

        [HttpGet]
        public async Task<ApiResponseData<List<Permission>>> GetAll()
        {
            try
            {
                var permissions = await _permissionService.GetAllPermissionsAsync();
                return ApiResponseData<List<Permission>>.Success(permissions, "Permissions retrieved successfully");
            }
            catch (Exception) { return ApiResponseData<List<Permission>>.Error("An error occurred", 500); }
        }
    }
}

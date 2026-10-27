using PhotographyPortfolioAPI.Contracts.Models.Auth;
using PhotographyPortfolioAPI.Infrastructure.Interfaces.Auth;
using PhotographyPortfolioAPI.Interfaces.Auth;

namespace PhotographyPortfolioAPI.Services.Auth
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepo;

        public PermissionService(IPermissionRepository permissionRepo)
        {
            _permissionRepo = permissionRepo;
        }

        public async Task<Guid> CreatePermissionAsync(PermissionCreateRequest request)
        {
            var permissionId = Guid.NewGuid();
            await _permissionRepo.CreateAsync(new Permission
            {
                permission_id = permissionId,
                permission_code = request.permission_code,
                permission_name = request.permission_name
            });
            return permissionId;
        }

        public async Task<List<Permission>> GetAllPermissionsAsync()
            => await _permissionRepo.GetAllAsync();
    }
}

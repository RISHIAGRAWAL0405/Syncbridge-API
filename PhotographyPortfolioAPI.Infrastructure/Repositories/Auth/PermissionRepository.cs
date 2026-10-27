using Dapper;
using PhotographyPortfolioAPI.Contracts.Models.Auth;
using PhotographyPortfolioAPI.Infrastructure.Base;
using PhotographyPortfolioAPI.Infrastructure.Interfaces.Auth;
using Microsoft.Extensions.Configuration;

namespace PhotographyPortfolioAPI.Infrastructure.Repositories.Auth
{
    public class PermissionRepository : BaseRepository, IPermissionRepository
    {
        public PermissionRepository(IConfiguration config) : base(config) { }

        public async Task CreateAsync(Permission permission)
        {
            using var con = CreateConnection();
            await con.ExecuteAsync(
                "INSERT INTO auth.permissions(permission_id,permission_code,permission_name) VALUES (@permission_id,@permission_code,@permission_name)",
                permission);
        }

        public async Task<List<Permission>> GetAllAsync()
        {
            using var con = CreateConnection();
            return (await con.QueryAsync<Permission>("SELECT * FROM auth.permissions WHERE is_assignable=true")).ToList();
        }
    }
}

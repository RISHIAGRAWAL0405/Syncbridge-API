using Dapper;
using PhotographyPortfolioAPI.Contracts.Models.Auth;
using PhotographyPortfolioAPI.Infrastructure.Base;
using PhotographyPortfolioAPI.Infrastructure.Interfaces.Auth;
using Microsoft.Extensions.Configuration;

namespace PhotographyPortfolioAPI.Infrastructure.Repositories.Auth
{
    public class RoleRepository : BaseRepository, IRoleRepository
    {
        public RoleRepository(IConfiguration config) : base(config) { }

        public async Task CreateAsync(Role role)
        {
            using var con = CreateConnection();
            await con.ExecuteAsync(
                "INSERT INTO auth.roles(role_id,role_code,role_name) VALUES (@role_id,@role_code,@role_name)", role);
        }

        public async Task UpdateAsync(Role role)
        {
            using var con = CreateConnection();
            await con.ExecuteAsync("UPDATE auth.roles SET role_name=@role_name WHERE role_id=@role_id", role);
        }

        public async Task<Role?> GetByIdAsync(Guid roleId)
        {
            using var con = CreateConnection();
            return await con.QueryFirstOrDefaultAsync<Role>("SELECT * FROM auth.roles WHERE role_id=@roleId", new { roleId });
        }

        public async Task<List<Role>> GetAllAsync()
        {
            using var con = CreateConnection();
            return (await con.QueryAsync<Role>("SELECT * FROM auth.roles WHERE is_assignable=true")).ToList();
        }

        public async Task<Role?> GetByNameAsync(string roleName)
        {
            using var con = CreateConnection();
            return await con.QueryFirstOrDefaultAsync<Role>("SELECT * FROM auth.roles WHERE role_name=@roleName", new { roleName });
        }

        public async Task RemoveAllPermissionsAsync(Guid roleId)
        {
            using var con = CreateConnection();
            await con.ExecuteAsync("DELETE FROM auth.role_permissions WHERE role_id=@roleId", new { roleId });
        }

        public async Task AddPermissionAsync(UpdateRolePermissionsDetails d)
        {
            var sql = @"INSERT INTO auth.role_permissions(role_permission_id,role_id,permission_id,can_view,can_add,can_update,can_delete,can_export)
                    VALUES (gen_random_uuid(),@role_id,@permission_id,@can_view,@can_add,@can_update,@can_delete,@can_export)";
            using var con = CreateConnection();
            await con.ExecuteAsync(sql, d);
        }

        public async Task<List<Permission>> GetRolePermissionsAsync(Guid roleId)
        {
            var sql = @"SELECT p.*, rp.can_view, rp.can_add, rp.can_update, rp.can_delete, rp.can_export
                        FROM auth.permissions p
                        JOIN auth.role_permissions rp ON p.permission_id = rp.permission_id
                        WHERE rp.role_id = @roleId
                        AND (rp.can_view=true OR rp.can_add=true OR rp.can_update=true OR rp.can_delete=true OR rp.can_export=true)";
            using var con = CreateConnection();
            return (await con.QueryAsync<Permission>(sql, new { roleId })).ToList();
        }
    }
}

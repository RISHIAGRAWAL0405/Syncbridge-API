using Dapper;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Contracts.Models.Project;
using PhotographyPortfolioAPI.Infrastructure.Base;
using PhotographyPortfolioAPI.Infrastructure.Interfaces.Project;
using Microsoft.Extensions.Configuration;

namespace PhotographyPortfolioAPI.Infrastructure.Repositories.Project
{
    public class ClientRepository : BaseRepository, IClientRepository
    {
        public ClientRepository(IConfiguration config) : base(config) { }

        public async Task<Guid> CreateAsync(ClientMaster client)
        {
            var sql = @"INSERT INTO client_master(client_id, client_name, email, mobile, company_name, address, is_active, created_at)
                        VALUES (@client_id, @client_name, @email, @mobile, @company_name, @address, @is_active, @created_at)";
            using var con = CreateConnection();
            await con.ExecuteAsync(sql, client);
            return client.client_id;
        }

        public async Task UpdateAsync(ClientMaster client)
        {
            var sql = @"UPDATE client_master
                        SET client_name=@client_name, email=@email, mobile=@mobile,
                            company_name=@company_name, address=@address, updated_at=@updated_at
                        WHERE client_id=@client_id";
            using var con = CreateConnection();
            await con.ExecuteAsync(sql, client);
        }

        public async Task<ClientMaster?> GetByIdAsync(Guid clientId)
        {
            using var con = CreateConnection();
            return await con.QueryFirstOrDefaultAsync<ClientMaster>(
                "SELECT * FROM client_master WHERE client_id=@clientId", new { clientId });
        }

        public async Task<List<ClientDto>> GetAllAsync()
        {
            var sql = @"SELECT c.*, COUNT(p.project_id) AS project_count
                        FROM client_master c
                        LEFT JOIN project_master p ON c.client_id = p.client_id AND p.is_active = true
                        GROUP BY c.client_id
                        ORDER BY c.client_name";
            using var con = CreateConnection();
            return (await con.QueryAsync<ClientDto>(sql)).ToList();
        }

        public async Task<List<ClientDto>> GetAllActiveAsync()
        {
            var sql = @"SELECT c.*, COUNT(p.project_id) AS project_count
                        FROM client_master c
                        LEFT JOIN project_master p ON c.client_id = p.client_id AND p.is_active = true
                        WHERE c.is_active = true
                        GROUP BY c.client_id
                        ORDER BY c.client_name";
            using var con = CreateConnection();
            return (await con.QueryAsync<ClientDto>(sql)).ToList();
        }

        public async Task ActiveInactiveAsync(Guid clientId, bool isActive)
        {
            using var con = CreateConnection();
            await con.ExecuteAsync(
                "UPDATE client_master SET is_active=@isActive WHERE client_id=@clientId",
                new { clientId, isActive });
        }
    }
}

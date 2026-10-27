using Dapper;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Contracts.Models.Project;
using PhotographyPortfolioAPI.Infrastructure.Base;
using PhotographyPortfolioAPI.Infrastructure.Interfaces.Project;
using Microsoft.Extensions.Configuration;

namespace PhotographyPortfolioAPI.Infrastructure.Repositories.Project
{
    public class WorkflowStepRepository : BaseRepository, IWorkflowStepRepository
    {
        public WorkflowStepRepository(IConfiguration config) : base(config) { }

        public async Task<Guid> CreateAsync(WorkflowStepMaster step)
        {
            var sql = @"INSERT INTO workflow_step_master(workflow_step_id, workflow_step_name, workflow_step_code, description, is_active, created_at)
                        VALUES (@workflow_step_id, @workflow_step_name, @workflow_step_code, @description, @is_active, @created_at)";
            using var con = CreateConnection();
            await con.ExecuteAsync(sql, step);
            return step.workflow_step_id;
        }

        public async Task UpdateAsync(WorkflowStepMaster step)
        {
            var sql = @"UPDATE workflow_step_master
                        SET workflow_step_name=@workflow_step_name, description=@description, updated_at=@updated_at
                        WHERE workflow_step_id=@workflow_step_id";
            using var con = CreateConnection();
            await con.ExecuteAsync(sql, step);
        }

        public async Task<WorkflowStepMaster?> GetByIdAsync(Guid stepId)
        {
            using var con = CreateConnection();
            return await con.QueryFirstOrDefaultAsync<WorkflowStepMaster>(
                "SELECT * FROM workflow_step_master WHERE workflow_step_id=@stepId", new { stepId });
        }

        public async Task<List<WorkflowStepDto>> GetAllAsync()
        {
            using var con = CreateConnection();
            return (await con.QueryAsync<WorkflowStepDto>(
                "SELECT * FROM workflow_step_master ORDER BY workflow_step_name")).ToList();
        }

        public async Task<List<WorkflowStepDto>> GetAllActiveAsync()
        {
            using var con = CreateConnection();
            return (await con.QueryAsync<WorkflowStepDto>(
                "SELECT * FROM workflow_step_master WHERE is_active=true ORDER BY workflow_step_name")).ToList();
        }

        public async Task ActiveInactiveAsync(Guid stepId, bool isActive)
        {
            using var con = CreateConnection();
            await con.ExecuteAsync(
                "UPDATE workflow_step_master SET is_active=@isActive WHERE workflow_step_id=@stepId",
                new { stepId, isActive });
        }
    }
}

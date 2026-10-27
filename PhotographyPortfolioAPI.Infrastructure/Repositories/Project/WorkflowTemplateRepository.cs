using Dapper;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Contracts.Models.Project;
using PhotographyPortfolioAPI.Infrastructure.Base;
using PhotographyPortfolioAPI.Infrastructure.Interfaces.Project;
using Microsoft.Extensions.Configuration;

namespace PhotographyPortfolioAPI.Infrastructure.Repositories.Project
{
    public class WorkflowTemplateRepository : BaseRepository, IWorkflowTemplateRepository
    {
        public WorkflowTemplateRepository(IConfiguration config) : base(config) { }

        public async Task<Guid> CreateAsync(WorkflowTemplate template)
        {
            var sql = @"INSERT INTO workflow_template_master(workflow_template_id, workflow_template_name, workflow_template_code, description, is_active, created_at)
                        VALUES (@workflow_template_id, @workflow_template_name, @workflow_template_code, @description, @is_active, @created_at)";
            using var con = CreateConnection();
            await con.ExecuteAsync(sql, template);
            return template.workflow_template_id;
        }

        public async Task UpdateAsync(WorkflowTemplate template)
        {
            var sql = @"UPDATE workflow_template_master
                        SET workflow_template_name=@workflow_template_name, description=@description, updated_at=@updated_at
                        WHERE workflow_template_id=@workflow_template_id";
            using var con = CreateConnection();
            await con.ExecuteAsync(sql, template);
        }

        public async Task<WorkflowTemplate?> GetByIdAsync(Guid templateId)
        {
            using var con = CreateConnection();
            return await con.QueryFirstOrDefaultAsync<WorkflowTemplate>(
                "SELECT * FROM workflow_template_master WHERE workflow_template_id=@templateId", new { templateId });
        }

        public async Task<List<WorkflowTemplateDto>> GetAllAsync()
        {
            using var con = CreateConnection();
            return (await con.QueryAsync<WorkflowTemplateDto>(
                "SELECT * FROM workflow_template_master ORDER BY workflow_template_name")).ToList();
        }

        public async Task<WorkflowTemplateDto?> GetWithStepsByIdAsync(Guid templateId)
        {
            var sql = @"SELECT workflow_template_id, workflow_template_name, workflow_template_code, description, is_active, created_at
                        FROM workflow_template_master
                        WHERE workflow_template_id = @templateId";

            var stepsSql = @"SELECT ts.workflow_template_step_id, ts.workflow_step_id,
                                    s.workflow_step_name, s.workflow_step_code, s.description,
                                    ts.display_order, ts.is_required
                             FROM workflow_template_step ts
                             JOIN workflow_step_master s ON ts.workflow_step_id = s.workflow_step_id
                             WHERE ts.workflow_template_id = @templateId
                             ORDER BY ts.display_order";

            using var con = CreateConnection();
            var template = await con.QueryFirstOrDefaultAsync<WorkflowTemplateDto>(sql, new { templateId });
            if (template == null) return null;

            template.steps = (await con.QueryAsync<WorkflowTemplateStepDto>(stepsSql, new { templateId })).ToList();
            return template;
        }

        public async Task ActiveInactiveAsync(Guid templateId, bool isActive)
        {
            using var con = CreateConnection();
            await con.ExecuteAsync(
                "UPDATE workflow_template_master SET is_active=@isActive WHERE workflow_template_id=@templateId",
                new { templateId, isActive });
        }

        public async Task AddStepAsync(WorkflowTemplateStep step)
        {
            var sql = @"INSERT INTO workflow_template_step(workflow_template_step_id, workflow_template_id, workflow_step_id, display_order, is_required, created_at)
                        VALUES (@workflow_template_step_id, @workflow_template_id, @workflow_step_id, @display_order, @is_required, @created_at)";
            using var con = CreateConnection();
            await con.ExecuteAsync(sql, step);
        }

        public async Task RemoveAllStepsAsync(Guid templateId)
        {
            using var con = CreateConnection();
            await con.ExecuteAsync(
                "DELETE FROM workflow_template_step WHERE workflow_template_id=@templateId", new { templateId });
        }

        public async Task<List<WorkflowTemplateStepDto>> GetStepsByTemplateIdAsync(Guid templateId)
        {
            var sql = @"SELECT ts.workflow_template_step_id, ts.workflow_step_id,
                               s.workflow_step_name, s.workflow_step_code, s.description,
                               ts.display_order, ts.is_required
                        FROM workflow_template_step ts
                        JOIN workflow_step_master s ON ts.workflow_step_id = s.workflow_step_id
                        WHERE ts.workflow_template_id = @templateId
                        ORDER BY ts.display_order";
            using var con = CreateConnection();
            return (await con.QueryAsync<WorkflowTemplateStepDto>(sql, new { templateId })).ToList();
        }
    }
}

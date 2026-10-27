using Dapper;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Contracts.Models.Project;
using PhotographyPortfolioAPI.Infrastructure.Base;
using PhotographyPortfolioAPI.Infrastructure.Interfaces.Project;
using Microsoft.Extensions.Configuration;

namespace PhotographyPortfolioAPI.Infrastructure.Repositories.Project
{
    public class StepLinkRepository : BaseRepository, IStepLinkRepository
    {
        public StepLinkRepository(IConfiguration config) : base(config) { }

        public async Task<Guid> CreateAsync(TaskWorkflowStepLink link)
        {
            var sql = @"INSERT INTO task_workflow_step_link
                        (link_id, task_workflow_step_id, task_id, category, title, url, link_type,
                         description, status, uploaded_by, is_active, created_at)
                        VALUES
                        (@link_id, @task_workflow_step_id, @task_id, @category, @title, @url, @link_type,
                         @description, @status, @uploaded_by, @is_active, @created_at)";
            using var con = CreateConnection();
            await con.ExecuteAsync(sql, link);
            return link.link_id;
        }

        public async Task UpdateAsync(TaskWorkflowStepLink link)
        {
            var sql = @"UPDATE task_workflow_step_link
                        SET category=@category, title=@title, url=@url, link_type=@link_type,
                            description=@description, updated_at=@updated_at
                        WHERE link_id=@link_id";
            using var con = CreateConnection();
            await con.ExecuteAsync(sql, link);
        }

        public async Task<TaskWorkflowStepLink?> GetByIdAsync(Guid linkId)
        {
            using var con = CreateConnection();
            return await con.QueryFirstOrDefaultAsync<TaskWorkflowStepLink>(
                "SELECT * FROM task_workflow_step_link WHERE link_id=@linkId AND is_active=true",
                new { linkId });
        }

        public async Task<List<StepLinkDto>> GetByStepIdAsync(Guid taskWorkflowStepId)
        {
            var sql = @"SELECT link_id, task_workflow_step_id, task_id, category, title, url, link_type,
                               description, status, status_note, uploaded_by, reviewed_by, reviewed_at, created_at
                        FROM task_workflow_step_link
                        WHERE task_workflow_step_id=@taskWorkflowStepId AND is_active=true
                        ORDER BY created_at DESC";
            using var con = CreateConnection();
            return (await con.QueryAsync<StepLinkDto>(sql, new { taskWorkflowStepId })).ToList();
        }

        public async Task<List<StepLinkDto>> GetByTaskIdAsync(Guid taskId)
        {
            var sql = @"SELECT link_id, task_workflow_step_id, task_id, category, title, url, link_type,
                               description, status, status_note, uploaded_by, reviewed_by, reviewed_at, created_at
                        FROM task_workflow_step_link
                        WHERE task_id=@taskId AND is_active=true
                        ORDER BY created_at DESC";
            using var con = CreateConnection();
            return (await con.QueryAsync<StepLinkDto>(sql, new { taskId })).ToList();
        }

        public async Task ReviewAsync(Guid linkId, string status, string? statusNote, Guid reviewedBy)
        {
            var sql = @"UPDATE task_workflow_step_link
                        SET status=@status, status_note=@statusNote,
                            reviewed_by=@reviewedBy, reviewed_at=@reviewedAt, updated_at=@reviewedAt
                        WHERE link_id=@linkId";
            using var con = CreateConnection();
            await con.ExecuteAsync(sql, new { linkId, status, statusNote, reviewedBy, reviewedAt = DateTime.UtcNow });
        }

        public async Task DeleteAsync(Guid linkId)
        {
            using var con = CreateConnection();
            await con.ExecuteAsync(
                "UPDATE task_workflow_step_link SET is_active=false, updated_at=@now WHERE link_id=@linkId",
                new { linkId, now = DateTime.UtcNow });
        }
    }
}

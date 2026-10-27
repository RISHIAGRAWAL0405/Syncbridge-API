using Dapper;
using Microsoft.Extensions.Configuration;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Infrastructure.Base;
using PhotographyPortfolioAPI.Infrastructure.Interfaces.Project;

namespace PhotographyPortfolioAPI.Infrastructure.Repositories.Project
{
    public class ClientDashboardRepository : BaseRepository, IClientDashboardRepository
    {
        public ClientDashboardRepository(IConfiguration config) : base(config) { }

        public async Task<ClientDashboardDto> GetDashboardAsync(Guid clientId)
        {
            using var con = CreateConnection();
            con.Open();

            var projects = (await con.QueryAsync<dynamic>(@"
                SELECT p.project_id, p.project_name, p.project_code, p.status,
                       p.start_date, p.end_date
                FROM project_master p
                WHERE p.client_id = @clientId AND p.is_active = true
                ORDER BY p.created_at DESC", new { clientId })).ToList();

            var tasks = (await con.QueryAsync<dynamic>(@"
                SELECT t.task_id, t.project_id, t.status
                FROM task_master t
                JOIN project_master p ON t.project_id = p.project_id
                WHERE p.client_id = @clientId AND p.is_active = true", new { clientId })).ToList();

            var now = DateTime.UtcNow;
            var steps = (await con.QueryAsync<dynamic>(@"
                SELECT tws.task_id, tws.end_date, tws.status
                FROM task_workflow_step tws
                JOIN task_master t ON tws.task_id = t.task_id
                JOIN project_master p ON t.project_id = p.project_id
                WHERE p.client_id = @clientId AND p.is_active = true", new { clientId })).ToList();

            var pendingLinks = (await con.QueryAsync<dynamic>(@"
                SELECT sl.link_id, sl.title, sl.url, sl.link_type, sl.category, sl.created_at,
                       tws.step_name, t.task_name, p.project_name, p.project_id
                FROM task_workflow_step_link sl
                JOIN task_workflow_step tws ON sl.task_workflow_step_id = tws.task_workflow_step_id
                JOIN task_master t ON sl.task_id = t.task_id
                JOIN project_master p ON t.project_id = p.project_id
                WHERE p.client_id = @clientId
                  AND sl.status = 'PENDING'
                  AND sl.is_active = true
                ORDER BY sl.created_at DESC", new { clientId })).ToList();

            var stepsByProject = (await con.QueryAsync<dynamic>(@"
                SELECT t.project_id,
                       COUNT(*) AS total_steps,
                       SUM(CASE WHEN tws.status = 'COMPLETED' THEN 1 ELSE 0 END) AS completed_steps
                FROM task_workflow_step tws
                JOIN task_master t ON tws.task_id = t.task_id
                JOIN project_master p ON t.project_id = p.project_id
                WHERE p.client_id = @clientId AND p.is_active = true
                GROUP BY t.project_id", new { clientId })).ToList();

            var projectSummaries = projects.Select(p =>
            {
                var pTasks = tasks.Where(t => (Guid)t.project_id == (Guid)p.project_id).ToList();
                var pSteps = stepsByProject.FirstOrDefault(s => (Guid)s.project_id == (Guid)p.project_id);
                int totalSteps = pSteps != null ? (int)(long)pSteps.total_steps : 0;
                int completedSteps = pSteps != null ? (int)(long)pSteps.completed_steps : 0;
                int progress = totalSteps == 0 ? 0 : (int)Math.Floor((double)completedSteps / totalSteps * 100);

                return new ClientProjectSummary
                {
                    ProjectId = (Guid)p.project_id,
                    ProjectName = (string)p.project_name,
                    ProjectCode = (string)(p.project_code ?? ""),
                    Status = (string)p.status,
                    StartDate = (DateTime?)p.start_date,
                    EndDate = (DateTime?)p.end_date,
                    TotalTasks = pTasks.Count,
                    CompletedTasks = pTasks.Count(t => (string)t.status == "COMPLETED"),
                    Progress = progress
                };
            }).ToList();

            var overdueTaskIds = steps
                .Where(s => (DateTime?)s.end_date < now && !new[] { "COMPLETED", "SKIPPED" }.Contains((string)s.status))
                .Select(s => (Guid)s.task_id)
                .Distinct()
                .Count();

            int totalAllSteps = stepsByProject.Sum(s => (int)(long)s.total_steps);
            int completedAllSteps = stepsByProject.Sum(s => (int)(long)s.completed_steps);
            int overallProgress = totalAllSteps == 0 ? 0 : (int)Math.Floor((double)completedAllSteps / totalAllSteps * 100);

            return new ClientDashboardDto
            {
                TotalProjects = projects.Count,
                ActiveProjects = projects.Count(p => (string)p.status == "IN_PROGRESS"),
                TotalTasks = tasks.Count,
                CompletedTasks = tasks.Count(t => (string)t.status == "COMPLETED"),
                InProgressTasks = tasks.Count(t => (string)t.status == "IN_PROGRESS"),
                OverdueTasks = overdueTaskIds,
                PendingApprovals = pendingLinks.Count,
                OverallProgress = overallProgress,
                Projects = projectSummaries,
                PendingLinks = pendingLinks.Select(l => new ClientPendingLinkItem
                {
                    LinkId = (Guid)l.link_id,
                    Title = (string)l.title,
                    Url = (string)l.url,
                    LinkType = (string)l.link_type,
                    Category = (string)l.category,
                    StepName = (string)l.step_name,
                    TaskName = (string)l.task_name,
                    ProjectName = (string)l.project_name,
                    ProjectId = (Guid)l.project_id,
                    CreatedAt = (DateTime)l.created_at
                }).ToList()
            };
        }
    }
}

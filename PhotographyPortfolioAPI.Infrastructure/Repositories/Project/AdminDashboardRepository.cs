using Dapper;
using Microsoft.Extensions.Configuration;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Infrastructure.Base;
using PhotographyPortfolioAPI.Infrastructure.Interfaces.Project;

namespace PhotographyPortfolioAPI.Infrastructure.Repositories.Project
{
    public class AdminDashboardRepository : BaseRepository, IAdminDashboardRepository
    {
        public AdminDashboardRepository(IConfiguration config) : base(config) { }

        public async Task<AdminDashboardDto> GetDashboardAsync()
        {
            using var con = CreateConnection();
            con.Open();

            var users = (await con.QueryAsync<dynamic>(
                "SELECT is_active FROM auth.users WHERE user_type != 'CLIENT'")).ToList();

            var clients = (await con.QueryAsync<dynamic>(
                "SELECT is_active FROM client_master")).ToList();

            var projects = (await con.QueryAsync<dynamic>(
                "SELECT project_id, status FROM project_master WHERE is_active = true")).ToList();

            var tasks = (await con.QueryAsync<dynamic>(
                "SELECT task_id, status FROM task_master WHERE is_active = true")).ToList();

            var now = DateTime.UtcNow;
            var overdueSteps = (await con.QueryAsync<dynamic>(@"
                SELECT DISTINCT tws.task_id
                FROM task_workflow_step tws
                WHERE tws.end_date < @now
                  AND tws.status NOT IN ('COMPLETED','SKIPPED')", new { now })).ToList();

            var pendingLinks = (await con.QueryAsync<dynamic>(@"
                SELECT sl.link_id, sl.title, sl.url, sl.link_type, sl.created_at,
                       tws.step_name, t.task_name, p.project_name, p.project_id,
                       c.client_name
                FROM task_workflow_step_link sl
                JOIN task_workflow_step tws ON sl.task_workflow_step_id = tws.task_workflow_step_id
                JOIN task_master t ON sl.task_id = t.task_id
                JOIN project_master p ON t.project_id = p.project_id
                JOIN client_master c ON p.client_id = c.client_id
                WHERE sl.status = 'PENDING' AND sl.is_active = true
                ORDER BY sl.created_at DESC
                LIMIT 20")).ToList();

            var recentProjects = (await con.QueryAsync<dynamic>(@"
                SELECT p.project_id, p.project_name, p.project_code, p.status, p.end_date,
                       c.client_name,
                       COUNT(DISTINCT t.task_id) AS total_tasks,
                       COUNT(DISTINCT CASE WHEN t.status = 'COMPLETED' THEN t.task_id END) AS completed_tasks,
                       COUNT(tws.task_workflow_step_id) AS total_steps,
                       SUM(CASE WHEN tws.status = 'COMPLETED' THEN 1 ELSE 0 END) AS completed_steps
                FROM project_master p
                JOIN client_master c ON p.client_id = c.client_id
                LEFT JOIN task_master t ON t.project_id = p.project_id
                LEFT JOIN task_workflow_step tws ON tws.task_id = t.task_id
                WHERE p.is_active = true
                GROUP BY p.project_id, p.project_name, p.project_code, p.status, p.end_date, c.client_name
                ORDER BY p.created_at DESC
                LIMIT 8")).ToList();

            var stepCounts = (await con.QueryAsync<dynamic>(@"
                SELECT COUNT(*) AS total,
                       SUM(CASE WHEN status = 'COMPLETED' THEN 1 ELSE 0 END) AS completed
                FROM task_workflow_step")).FirstOrDefault();

            long totalSteps = stepCounts?.total ?? 0;
            long completedSteps = stepCounts?.completed ?? 0;
            int overallProgress = totalSteps == 0 ? 0 : (int)Math.Floor((double)completedSteps / totalSteps * 100);

            return new AdminDashboardDto
            {
                TotalUsers = users.Count,
                ActiveUsers = users.Count(u => (bool)u.is_active),
                TotalClients = clients.Count,
                ActiveClients = clients.Count(c => (bool)c.is_active),
                TotalProjects = projects.Count,
                TotalTasks = tasks.Count,
                CompletedTasks = tasks.Count(t => (string)t.status == "COMPLETED"),
                InProgressTasks = tasks.Count(t => (string)t.status == "IN_PROGRESS"),
                OverdueTasks = overdueSteps.Count,
                PendingLinks = pendingLinks.Count,
                OverallProgress = overallProgress,
                ProjectsByStatus = projects
                    .GroupBy(p => (string)p.status)
                    .Select(g => new AdminStatusCount { Status = g.Key, Count = g.Count() })
                    .OrderBy(s => s.Status).ToList(),
                TasksByStatus = tasks
                    .GroupBy(t => (string)t.status)
                    .Select(g => new AdminStatusCount { Status = g.Key, Count = g.Count() })
                    .OrderBy(s => s.Status).ToList(),
                RecentProjects = recentProjects.Select(p =>
                {
                    int ts = (int)(long)p.total_steps;
                    int cs = (int)(long)p.completed_steps;
                    return new AdminRecentProjectItem
                    {
                        ProjectId = (Guid)p.project_id,
                        ProjectName = (string)p.project_name,
                        ProjectCode = (string)(p.project_code ?? ""),
                        ClientName = (string)p.client_name,
                        Status = (string)p.status,
                        Progress = ts == 0 ? 0 : (int)Math.Floor((double)cs / ts * 100),
                        TotalTasks = (int)(long)p.total_tasks,
                        CompletedTasks = (int)(long)p.completed_tasks,
                        EndDate = (DateTime?)p.end_date
                    };
                }).ToList(),
                PendingLinkItems = pendingLinks.Select(l => new AdminPendingLinkItem
                {
                    LinkId = (Guid)l.link_id,
                    Title = (string)l.title,
                    Url = (string)l.url,
                    LinkType = (string)l.link_type,
                    StepName = (string)l.step_name,
                    TaskName = (string)l.task_name,
                    ProjectName = (string)l.project_name,
                    ClientName = (string)l.client_name,
                    ProjectId = (Guid)l.project_id,
                    CreatedAt = (DateTime)l.created_at
                }).ToList()
            };
        }
    }
}

using Dapper;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Contracts.Models.Project;
using PhotographyPortfolioAPI.Infrastructure.Base;
using PhotographyPortfolioAPI.Infrastructure.Interfaces.Project;
using Microsoft.Extensions.Configuration;

namespace PhotographyPortfolioAPI.Infrastructure.Repositories.Project
{
    public class ProjectRepository : BaseRepository, IProjectRepository
    {
        public ProjectRepository(IConfiguration config) : base(config) { }

        public async Task<Guid> CreateAsync(ProjectMaster project)
        {
            var sql = @"INSERT INTO project_master(project_id, client_id, project_name, project_code, description,
                            start_date, end_date, status, is_active, created_at)
                        VALUES (@project_id, @client_id, @project_name, @project_code, @description,
                            @start_date, @end_date, @status, @is_active, @created_at)";
            using var con = CreateConnection();
            await con.ExecuteAsync(sql, project);
            return project.project_id;
        }

        public async Task UpdateAsync(ProjectMaster project)
        {
            var sql = @"UPDATE project_master
                        SET project_name=@project_name, project_code=@project_code, description=@description,
                            start_date=@start_date, end_date=@end_date, status=@status, updated_at=@updated_at
                        WHERE project_id=@project_id";
            using var con = CreateConnection();
            await con.ExecuteAsync(sql, project);
        }

        public async Task<ProjectMaster?> GetByIdAsync(Guid projectId)
        {
            using var con = CreateConnection();
            return await con.QueryFirstOrDefaultAsync<ProjectMaster>(
                "SELECT * FROM project_master WHERE project_id=@projectId", new { projectId });
        }

        public async Task<List<ProjectDto>> GetAllAsync()
        {
            var sql = @"SELECT p.*, c.client_name
                        FROM project_master p
                        JOIN client_master c ON p.client_id = c.client_id
                        ORDER BY p.created_at DESC";
            using var con = CreateConnection();
            return (await con.QueryAsync<ProjectDto>(sql)).ToList();
        }

        public async Task<List<ProjectDto>> GetByClientIdAsync(Guid clientId)
        {
            var sql = @"SELECT p.*, c.client_name
                        FROM project_master p
                        JOIN client_master c ON p.client_id = c.client_id
                        WHERE p.client_id = @clientId
                        ORDER BY p.created_at DESC";
            using var con = CreateConnection();
            return (await con.QueryAsync<ProjectDto>(sql, new { clientId })).ToList();
        }

        public async Task ActiveInactiveAsync(Guid projectId, bool isActive)
        {
            using var con = CreateConnection();
            await con.ExecuteAsync(
                "UPDATE project_master SET is_active=@isActive WHERE project_id=@projectId",
                new { projectId, isActive });
        }

        public async Task UpdateStatusAsync(Guid projectId, string status)
        {
            using var con = CreateConnection();
            await con.ExecuteAsync(
                "UPDATE project_master SET status=@status, updated_at=@updatedAt WHERE project_id=@projectId",
                new { projectId, status, updatedAt = DateTime.UtcNow });
        }

        public async Task<ProjectProfileDto> GetProjectProfileAsync(Guid projectId)
        {
            using var con = CreateConnection();

            // Get project
            var projectSql = @"SELECT p.project_id, p.project_name, p.project_code, p.description,
                                      p.start_date, p.end_date, p.status, p.is_active
                               FROM project_master p
                               WHERE p.project_id = @projectId";
            var project = await con.QueryFirstOrDefaultAsync<ProjectHeaderDto>(projectSql, new { projectId });
            if (project == null) return null;

            // Get client
            var clientSql = @"SELECT c.client_id, c.client_name, c.company_name
                              FROM client_master c
                              JOIN project_master p ON p.client_id = c.client_id
                              WHERE p.project_id = @projectId";
            var client = await con.QueryFirstOrDefaultAsync<ClientProfileDto>(clientSql, new { projectId });

            // Get all tasks with their workflow steps
            var tasksSql = @"SELECT t.task_id, t.task_name, t.task_code, wt.workflow_template_name,
                                    t.start_date, t.end_date, t.status
                             FROM task_master t
                             JOIN workflow_template_master wt ON t.workflow_template_id = wt.workflow_template_id
                             WHERE t.project_id = @projectId
                             ORDER BY t.created_at DESC";
            var tasks = (await con.QueryAsync<dynamic>(tasksSql, new { projectId })).ToList();

            var stepsSql = @"SELECT tws.task_workflow_step_id, tws.step_name, tws.display_order, tws.status,
                                    tws.start_date, tws.end_date, tws.task_id
                             FROM task_workflow_step tws
                             WHERE tws.task_id IN (SELECT task_id FROM task_master WHERE project_id = @projectId)
                             ORDER BY tws.task_id, tws.display_order";
            var steps = (await con.QueryAsync<dynamic>(stepsSql, new { projectId })).ToList();

            var linksSql = @"SELECT link_id, task_workflow_step_id, task_id, category, title, url, link_type,
                                    description, status, status_note, uploaded_by, reviewed_by, reviewed_at, created_at
                             FROM task_workflow_step_link
                             WHERE task_id IN (SELECT task_id FROM task_master WHERE project_id = @projectId)
                               AND is_active = true
                             ORDER BY created_at DESC";
            var links = (await con.QueryAsync<StepLinkDto>(linksSql, new { projectId })).ToList();

            // Build task profiles
            var taskProfiles = new List<TaskProfileDto>();
            var now = DateTime.UtcNow;

            foreach (var task in tasks)
            {
                var taskSteps = steps.Where(s => (Guid)s.task_id == (Guid)task.task_id).ToList();
                
                // Calculate current stage and progress
                string currentStage = null;
                int totalSteps = taskSteps.Count;
                int completedSteps = taskSteps.Count(s => (string)s.status == "COMPLETED");
                int inProgressSteps = taskSteps.Count(s => (string)s.status == "IN_PROGRESS");
                int notStartedSteps = taskSteps.Count(s => (string)s.status == "NOT_STARTED");

                var inProgressStep = taskSteps.FirstOrDefault(s => (string)s.status == "IN_PROGRESS");
                if (inProgressStep != null)
                    currentStage = (string)inProgressStep.step_name;
                else
                {
                    var firstNotStarted = taskSteps.FirstOrDefault(s => (string)s.status == "NOT_STARTED");
                    if (firstNotStarted != null)
                        currentStage = (string)firstNotStarted.step_name;
                }

                bool allDoneOrSkipped = taskSteps.All(s => (string)s.status == "COMPLETED" || (string)s.status == "SKIPPED");
                if (allDoneOrSkipped) currentStage = "Completed";

                // Use the persisted task status (manually set or auto-completed when all steps finish)
                string taskStatus = (string)task.status;

                // Check if overdue
                bool isOverdue = taskSteps.Any(s => (DateTime?)s.end_date < now && 
                                                     !new[] { "COMPLETED", "SKIPPED" }.Contains((string)s.status));

                var percentage = totalSteps == 0 ? 0 : (int)Math.Floor((double)completedSteps / totalSteps * 100);

                var taskProfile = new TaskProfileDto
                {
                    task_id = (Guid)task.task_id,
                    task_name = (string)task.task_name,
                    task_code = (string)task.task_code,
                    workflow_template_name = (string)task.workflow_template_name,
                    start_date = (DateTime?)task.start_date,
                    end_date = (DateTime?)task.end_date,
                    current_stage = currentStage,
                    status = taskStatus,
                    progress = new TaskProgressDto
                    {
                        total_steps = totalSteps,
                        completed_steps = completedSteps,
                        percentage = percentage
                    },
                    workflow_steps = taskSteps.Select(s => new TaskWorkflowStepProfileDto
                    {
                        task_workflow_step_id = (Guid)s.task_workflow_step_id,
                        step_name = (string)s.step_name,
                        display_order = (int)s.display_order,
                        status = (string)s.status,
                        start_date = (DateTime?)s.start_date,
                        end_date = (DateTime?)s.end_date,
                        links = links.Where(l => l.task_workflow_step_id == (Guid)s.task_workflow_step_id).ToList()
                    }).ToList()
                };

                taskProfiles.Add(taskProfile);
            }

            // Calculate project summary
            int totalTasks = taskProfiles.Count;
            int completedTasks = taskProfiles.Count(t => t.status == "COMPLETED");
            int inProgressTasks = taskProfiles.Count(t => t.status == "IN_PROGRESS");
            int notStartedTasks = taskProfiles.Count(t => t.status == "NEW" || t.status == "NOT_STARTED");
            int overdueTasks = taskProfiles.Count(t => t.workflow_steps.Any(s => s.end_date < now && !new[] { "COMPLETED", "SKIPPED" }.Contains(s.status)));

            // Calculate overall progress
            int totalWorkflowSteps = taskProfiles.Sum(t => t.progress.total_steps);
            int totalCompletedSteps = taskProfiles.Sum(t => t.progress.completed_steps);
            int overallProgress = totalWorkflowSteps == 0 ? 0 : (int)Math.Floor((double)totalCompletedSteps / totalWorkflowSteps * 100);

            var summary = new ProjectSummaryDto
            {
                total_tasks = totalTasks,
                completed_tasks = completedTasks,
                in_progress_tasks = inProgressTasks,
                not_started_tasks = notStartedTasks,
                overdue_tasks = overdueTasks,
                overall_progress = overallProgress
            };

            return new ProjectProfileDto
            {
                project = project,
                client = client,
                summary = summary,
                tasks = taskProfiles
            };
        }
    }
}

using Dapper;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Contracts.Models.Project;
using PhotographyPortfolioAPI.Infrastructure.Base;
using PhotographyPortfolioAPI.Infrastructure.Interfaces.Project;
using Microsoft.Extensions.Configuration;

namespace PhotographyPortfolioAPI.Infrastructure.Repositories.Project
{
    public class TaskRepository : BaseRepository, ITaskRepository
    {
        public TaskRepository(IConfiguration config) : base(config) { }

        public async Task<Guid> CreateAsync(TaskMaster task, List<TaskWorkflowStep> workflowSteps)
        {
            using var con = CreateConnection();
            con.Open();
            using var transaction = con.BeginTransaction();

            try
            {
                var taskSql = @"INSERT INTO task_master(task_id, project_id, workflow_template_id, task_name, task_code, description,
                                start_date, end_date, is_active, status, created_at)
                            VALUES (@task_id, @project_id, @workflow_template_id, @task_name, @task_code, @description,
                                @start_date, @end_date, @is_active, @status, @created_at)";
                await con.ExecuteAsync(taskSql, task, transaction);

                if (workflowSteps.Count > 0)
                {
                    var stepsSql = @"INSERT INTO task_workflow_step(task_workflow_step_id, task_id, workflow_step_id, step_name, 
                                        step_code, display_order, is_required, start_date, end_date, notes, status, started_at, completed_at, created_at)
                                    VALUES (@task_workflow_step_id, @task_id, @workflow_step_id, @step_name, 
                                        @step_code, @display_order, @is_required, @start_date, @end_date, @notes, @status, @started_at, @completed_at, @created_at)";
                    await con.ExecuteAsync(stepsSql, workflowSteps, transaction);
                }

                transaction.Commit();
                return task.task_id;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public async Task UpdateAsync(TaskMaster task)
        {
            var sql = @"UPDATE task_master
                        SET task_name=@task_name, task_code=@task_code, description=@description,
                            start_date=@start_date, end_date=@end_date, updated_at=@updated_at
                        WHERE task_id=@task_id";
            using var con = CreateConnection();
            await con.ExecuteAsync(sql, task);
        }

        public async Task UpdateStatusAsync(Guid taskId, string status)
        {
            using var con = CreateConnection();
            await con.ExecuteAsync(
                "UPDATE task_master SET status=@status, updated_at=@updatedAt WHERE task_id=@taskId",
                new { taskId, status, updatedAt = DateTime.UtcNow });
        }

        public async Task<TaskMaster?> GetByIdAsync(Guid taskId)
        {
            using var con = CreateConnection();
            return await con.QueryFirstOrDefaultAsync<TaskMaster>(
                "SELECT * FROM task_master WHERE task_id=@taskId", new { taskId });
        }

        public async Task<TaskDto?> GetWithWorkflowStepsByIdAsync(Guid taskId)
        {
            var sql = @"SELECT t.task_id, t.project_id, p.project_name, t.workflow_template_id, 
                               wt.workflow_template_name, t.task_name, t.task_code, t.description,
                               t.start_date, t.end_date, t.is_active, t.status, t.created_at
                        FROM task_master t
                        JOIN project_master p ON t.project_id = p.project_id
                        JOIN workflow_template_master wt ON t.workflow_template_id = wt.workflow_template_id
                        WHERE t.task_id = @taskId";

            var stepsSql = @"SELECT task_workflow_step_id, task_id, workflow_step_id, step_name, step_code,
                                    display_order, is_required, start_date, end_date, notes, status, started_at, completed_at
                             FROM task_workflow_step
                             WHERE task_id = @taskId
                             ORDER BY display_order";

            var linksSql = @"SELECT link_id, task_workflow_step_id, task_id, title, url, link_type,
                                    description, status, status_note, uploaded_by, reviewed_by, reviewed_at, created_at
                             FROM task_workflow_step_link
                             WHERE task_id = @taskId AND is_active = true
                             ORDER BY created_at DESC";

            using var con = CreateConnection();
            var task = await con.QueryFirstOrDefaultAsync<TaskDto>(sql, new { taskId });
            if (task == null) return null;

            var steps = (await con.QueryAsync<TaskWorkflowStepDto>(stepsSql, new { taskId })).ToList();
            var links = (await con.QueryAsync<StepLinkDto>(linksSql, new { taskId })).ToList();

            foreach (var step in steps)
                step.links = links.Where(l => l.task_workflow_step_id == step.task_workflow_step_id).ToList();

            task.workflow_steps = steps;
            return task;
        }

        public async Task<List<TaskDto>> GetAllAsync()
        {
            var sql = @"SELECT t.task_id, t.project_id, p.project_name, t.workflow_template_id,
                               wt.workflow_template_name, t.task_name, t.task_code, t.description,
                               t.start_date, t.end_date, t.is_active, t.status, t.created_at
                        FROM task_master t
                        JOIN project_master p ON t.project_id = p.project_id
                        JOIN workflow_template_master wt ON t.workflow_template_id = wt.workflow_template_id
                        ORDER BY t.created_at DESC";
            using var con = CreateConnection();
            return (await con.QueryAsync<TaskDto>(sql)).ToList();
        }

        public async Task<List<TaskDto>> GetByProjectIdAsync(Guid projectId)
        {
            var sql = @"SELECT t.task_id, t.project_id, p.project_name, t.workflow_template_id,
                               wt.workflow_template_name, t.task_name, t.task_code, t.description,
                               t.start_date, t.end_date, t.is_active, t.status, t.created_at
                        FROM task_master t
                        JOIN project_master p ON t.project_id = p.project_id
                        JOIN workflow_template_master wt ON t.workflow_template_id = wt.workflow_template_id
                        WHERE t.project_id = @projectId
                        ORDER BY t.created_at DESC";
            using var con = CreateConnection();
            return (await con.QueryAsync<TaskDto>(sql, new { projectId })).ToList();
        }

        public async Task DeleteAsync(Guid taskId)
        {
            using var con = CreateConnection();
            con.Open();
            using var transaction = con.BeginTransaction();

            try
            {
                // Delete workflow steps first (foreign key constraint)
                await con.ExecuteAsync(
                    "DELETE FROM task_workflow_step WHERE task_id=@taskId",
                    new { taskId }, transaction);

                // Delete task
                await con.ExecuteAsync(
                    "DELETE FROM task_master WHERE task_id=@taskId",
                    new { taskId }, transaction);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public async Task<List<TaskWorkflowStep>> GetWorkflowStepsByTaskIdAsync(Guid taskId)
        {
            var sql = @"SELECT * FROM task_workflow_step WHERE task_id=@taskId ORDER BY display_order";
            using var con = CreateConnection();
            return (await con.QueryAsync<TaskWorkflowStep>(sql, new { taskId })).ToList();
        }

        public async Task<TaskWorkflowStep?> GetWorkflowStepByIdAsync(Guid taskWorkflowStepId)
        {
            using var con = CreateConnection();
            return await con.QueryFirstOrDefaultAsync<TaskWorkflowStep>(
                "SELECT * FROM task_workflow_step WHERE task_workflow_step_id=@taskWorkflowStepId",
                new { taskWorkflowStepId });
        }

        public async Task UpdateWorkflowStepAsync(TaskWorkflowStep step)
        {
            var sql = @"UPDATE task_workflow_step
                        SET start_date=@start_date, end_date=@end_date, notes=@notes, status=@status, started_at=@started_at, completed_at=@completed_at, updated_at=@updated_at
                        WHERE task_workflow_step_id=@task_workflow_step_id";
            using var con = CreateConnection();
            await con.ExecuteAsync(sql, step);
        }

        public async Task UpdateMultipleWorkflowStepsAsync(List<TaskWorkflowStep> steps)
        {
            using var con = CreateConnection();
            con.Open();
            using var transaction = con.BeginTransaction();

            try
            {
                var sql = @"UPDATE task_workflow_step
                            SET start_date=@start_date, end_date=@end_date, notes=@notes, updated_at=@updated_at
                            WHERE task_workflow_step_id=@task_workflow_step_id";
                await con.ExecuteAsync(sql, steps, transaction);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public async Task<List<CalendarTaskDto>> GetCalendarTasksAsync(Guid clientId, int year, int month)
        {
            var startOfMonth = new DateTime(year, month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var sql = @"SELECT DISTINCT t.task_id, t.task_name, t.task_code, t.description,
                               t.project_id, p.project_name, t.start_date, t.end_date, t.status
                        FROM task_master t
                        JOIN project_master p ON t.project_id = p.project_id
                        LEFT JOIN task_workflow_step tws ON tws.task_id = t.task_id
                        WHERE p.client_id = @clientId
                          AND t.is_active = true
                          AND (
                            (t.start_date <= @endOfMonth AND t.end_date >= @startOfMonth)
                            OR (t.start_date BETWEEN @startOfMonth AND @endOfMonth)
                            OR (t.end_date BETWEEN @startOfMonth AND @endOfMonth)
                            OR (tws.start_date <= @endOfMonth AND tws.end_date >= @startOfMonth)
                            OR (tws.start_date BETWEEN @startOfMonth AND @endOfMonth)
                            OR (tws.end_date BETWEEN @startOfMonth AND @endOfMonth)
                          )
                        ORDER BY t.start_date";

            using var con = CreateConnection();
            var tasks = (await con.QueryAsync<CalendarTaskDto>(sql, new { clientId, startOfMonth, endOfMonth })).ToList();

            if (tasks.Count == 0) return tasks;

            var taskIds = tasks.Select(t => t.task_id).ToList();
            var stepsSql = @"SELECT task_workflow_step_id, task_id, workflow_step_id, step_name, step_code,
                                    display_order, is_required, start_date, end_date, notes, status, started_at, completed_at
                             FROM task_workflow_step
                             WHERE task_id = ANY(@taskIds)
                             ORDER BY display_order";
            var allSteps = (await con.QueryAsync<TaskWorkflowStepDto>(stepsSql, new { taskIds = taskIds.ToArray() })).ToList();

            foreach (var task in tasks)
            {
                task.workflow_steps = allSteps.Where(s => s.task_id == task.task_id).ToList();
                task.total_steps = task.workflow_steps.Count;
                task.completed_steps = task.workflow_steps.Count(s => s.status == "COMPLETED");
            }

            return tasks;
        }
    }
}

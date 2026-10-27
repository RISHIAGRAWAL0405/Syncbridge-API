using PhotographyPortfolioAPI.Infrastructure.Interfaces.Auth;
using PhotographyPortfolioAPI.Infrastructure.Interfaces.Project;
using PhotographyPortfolioAPI.Infrastructure.Repositories.Auth;
using PhotographyPortfolioAPI.Infrastructure.Repositories.Project;
using PhotographyPortfolioAPI.Infrastructure.Services.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace PhotographyPortfolioAPI.Infrastructure.Extensions
{
    public static class InfrastructureServiceExtension
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IJwtService, JwtService>();

            // Project Repositories
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IStepLinkRepository, StepLinkRepository>();
            services.AddScoped<IWorkflowStepRepository, WorkflowStepRepository>();
            services.AddScoped<IWorkflowTemplateRepository, WorkflowTemplateRepository>();
            services.AddScoped<IAdminDashboardRepository, AdminDashboardRepository>();
            services.AddScoped<IClientDashboardRepository, ClientDashboardRepository>();

            return services;
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using PhotographyPortfolioAPI.Helpers;
using PhotographyPortfolioAPI.Interfaces.Auth;
using PhotographyPortfolioAPI.Interfaces.Project;
using PhotographyPortfolioAPI.Services.Auth;
using PhotographyPortfolioAPI.Services.Project;

namespace PhotographyPortfolioAPI.Extensions
{
    public static class ApiServiceExtension
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IPermissionService, PermissionService>();

            // Project Services
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IStepLinkService, StepLinkService>();
            services.AddScoped<IWorkflowStepService, WorkflowStepService>();
            services.AddScoped<IWorkflowTemplateService, WorkflowTemplateService>();
            services.AddScoped<IAdminDashboardService, AdminDashboardService>();
            services.AddScoped<IClientDashboardService, ClientDashboardService>();

            return services;
        }
    }
}

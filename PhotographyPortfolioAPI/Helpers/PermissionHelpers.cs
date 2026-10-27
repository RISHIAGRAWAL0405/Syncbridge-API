using Microsoft.AspNetCore.Authorization;

namespace PhotographyPortfolioAPI.Helpers
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }
        public PermissionRequirement(string permission) => Permission = permission;
    }

    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var permissions = context.User.Claims
                .Where(c => c.Type == "permission")
                .Select(c => c.Value);

            if (permissions.Contains(requirement.Permission))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }

    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly DefaultAuthorizationPolicyProvider _fallback;

        public PermissionPolicyProvider(Microsoft.Extensions.Options.IOptions<AuthorizationOptions> options)
        {
            _fallback = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => _fallback.GetDefaultPolicyAsync();
        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => _fallback.GetFallbackPolicyAsync();

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement(policyName))
                .Build();
            return Task.FromResult<AuthorizationPolicy?>(policy);
        }
    }
}

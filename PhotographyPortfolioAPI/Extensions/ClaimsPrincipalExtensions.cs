using System.Security.Claims;

namespace PhotographyPortfolioAPI.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user.FindFirst("user_id")?.Value;
            if (string.IsNullOrEmpty(userId)) throw new UnauthorizedAccessException("User ID claim not found.");
            return Guid.Parse(userId);
        }
    }
}

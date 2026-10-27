using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PhotographyPortfolioAPI.Contracts.Models.Auth;
using PhotographyPortfolioAPI.Infrastructure.Interfaces.Auth;

namespace PhotographyPortfolioAPI.Infrastructure.Services.Auth
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateAccessTokenAsync(User user, List<UserPermissionsData> permissions)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.user_id.ToString()),
                new Claim("user_id", user.user_id.ToString()),
                new Claim(ClaimTypes.Name, user.user_name)
            };
            permissions.ForEach(p => claims.Add(new Claim("permission", p.permission_code)));

            var token = new JwtSecurityToken(
                _jwtSettings.Issuer, _jwtSettings.Audience, claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshTokenAsync()
            => Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }
}

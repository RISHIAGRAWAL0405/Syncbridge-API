using Dapper;
using PhotographyPortfolioAPI.Contracts.Models.Auth;
using PhotographyPortfolioAPI.Infrastructure.Base;
using PhotographyPortfolioAPI.Infrastructure.Interfaces.Auth;
using Microsoft.Extensions.Configuration;

namespace PhotographyPortfolioAPI.Infrastructure.Repositories.Auth
{
    public class TokenRepository : BaseRepository, ITokenRepository
    {
        public TokenRepository(IConfiguration config) : base(config) { }

        public async Task SaveRefreshTokenAsync(RefreshToken token)
        {
            var sql = @"INSERT INTO auth.refresh_tokens(refresh_token_id,user_id,token,expires_at,is_revoked)
                        VALUES (@refresh_token_id,@user_id,@token,@expires_at,@is_revoked)";
            using var con = CreateConnection();
            await con.ExecuteAsync(sql, token);
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
        {
            using var con = CreateConnection();
            return await con.QueryFirstOrDefaultAsync<RefreshToken>(
                "SELECT * FROM auth.refresh_tokens WHERE token=@token AND is_revoked=false", new { token });
        }

        public async Task Revoke(string token)
        {
            using var con = CreateConnection();
            await con.ExecuteAsync("UPDATE auth.refresh_tokens SET is_revoked=true WHERE token=@token", new { token });
        }
    }
}

using Microsoft.Extensions.Configuration;
using System.Data;

namespace PhotographyPortfolioAPI.Infrastructure.Base
{
    public abstract class BaseRepository
    {
        private readonly string _connection;
        protected BaseRepository(IConfiguration config)
        {
            _connection = config.GetConnectionString("DbConnection");
        }
        protected IDbConnection CreateConnection()
            => new Npgsql.NpgsqlConnection(_connection);
    }
}

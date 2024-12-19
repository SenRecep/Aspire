using System.Data;
using CompanyName.BuildingBlocks.Database.Base.Abstractions;
using Npgsql;

namespace CompanyName.BuildingBlocks.Database.PostgreSQL;
internal sealed class SqlConnectionFactory
    (string connectionString) : ISqlConnectionFactory
{
    public IDbConnection Create() => new NpgsqlConnection(connectionString);
}

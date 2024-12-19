using System.Data;
using CompanyName.BuildingBlocks.Database.Base.Abstractions;
using Npgsql;

namespace CompanyName.BuildingBlocks.Database.PostgreSQL;

/// <summary>
/// Factory for creating read-only connections to the database.
/// </summary>
/// <param name="connectionString"></param>
internal sealed class SqlReadOnlyConnectionFactory
    (string connectionString) : IReadOnlyConnectionFactory
{
    public IDbConnection Create() => new NpgsqlConnection(connectionString);
}

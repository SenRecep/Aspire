using System.Data;

namespace CompanyName.BuildingBlocks.Database.Base.Abstractions;
public interface ISqlConnectionFactory
{
    IDbConnection Create();
}

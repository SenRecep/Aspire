
using CSharpEssentials;
using Microsoft.EntityFrameworkCore;

namespace CompanyName.BuildingBlocks.Database.PostgreSQL;

public sealed class PostgresDbContextOptions
{
    public QuerySplittingBehavior QuerySplittingBehavior { get; set; } = QuerySplittingBehavior.SplitQuery;
    public QueryTrackingBehavior QueryTrackingBehavior { get; set; } = QueryTrackingBehavior.TrackAll;
    public bool EnableDetailedErrors { get; set; } = true;
    public bool EnableSensitiveDataLogging { get; set; } = true;
    public bool EnableAuditableInterceptor { get; set; } = true;
    public bool EnablePublishDomainEventsInterceptor { get; set; } = true;

    public Maybe<PostgresPoolOptions> PoolOptions { get; set; }
    public Maybe<PostgresRetryOptions> RetryOptions { get; set; } = new PostgresRetryOptions();
    public Maybe<string> MigrationsAssembly { get; set; }

    public sealed class PostgresRetryOptions
    {
        public int MaxRetryCount { get; set; } = 5;
        public TimeSpan MaxRetryDelay { get; set; } = TimeSpan.FromSeconds(30);
        public ICollection<string>? AdditionalErrorCodes { get; set; }
    }

    public sealed class PostgresPoolOptions
    {
        public int MaxPoolSize { get; set; } = 1024;
    }
}

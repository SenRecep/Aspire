using EntityFramework.Exceptions.PostgreSQL;
using CompanyName.BuildingBlocks.Application.Shared.Abstractions;
using CompanyName.BuildingBlocks.Database.Base.Abstractions;
using CompanyName.BuildingBlocks.Database.PostgreSQL.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OpenTelemetry.Trace;

namespace CompanyName.BuildingBlocks.Database.PostgreSQL;
public static class DependencyInjection
{
    public static IServiceCollection AddPostgresDbContext<TDbContext>(
        this IServiceCollection services,
        string connectionString,
        Action<PostgresDbContextOptions>? options = null,
        Action<IServiceProvider, DbContextOptionsBuilder>? configureOptions = null)
        where TDbContext : DbContext
    {
        var postgresOptions = new PostgresDbContextOptions();
        options?.Invoke(postgresOptions);

        if (postgresOptions.QueryTrackingBehavior == QueryTrackingBehavior.NoTracking)
            services.AddSingleton<IReadOnlyConnectionFactory, SqlReadOnlyConnectionFactory>(_ => new SqlReadOnlyConnectionFactory(connectionString));
        else
        {
            services.AddScoped<IUnitOfWork, EfUnitOfWork<TDbContext>>();
            services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
        }

        //services.AddDbContextFactory<TDbContext>(CreateDbContextOptionsBuilder(
        //    services,
        //    connectionString,
        //    postgresOptions,
        //    configureOptions));

        services.AddDbContext<TDbContext>(CreateDbContextOptionsBuilder(
            services,
            connectionString,
            postgresOptions,
            configureOptions));
        return services;
    }


    public static IServiceCollection AddPooledPostgresDbContext<TDbContext>(
        this IServiceCollection services,
        string connectionString,
        Action<PostgresDbContextOptions>? options = null,
        Action<IServiceProvider, DbContextOptionsBuilder>? configureOptions = null)
        where TDbContext : DbContext
    {
        var postgresOptions = new PostgresDbContextOptions()
        {
            PoolOptions = new PostgresDbContextOptions.PostgresPoolOptions()
        };
        options?.Invoke(postgresOptions);
        if (postgresOptions.PoolOptions.HasNoValue)
            postgresOptions.PoolOptions = new PostgresDbContextOptions.PostgresPoolOptions();

        if (postgresOptions.QueryTrackingBehavior == QueryTrackingBehavior.NoTracking)
            services.AddSingleton<IReadOnlyConnectionFactory, SqlReadOnlyConnectionFactory>(_ => new SqlReadOnlyConnectionFactory(connectionString));
        else
        {
            services.AddScoped<IUnitOfWork, EfUnitOfWork<TDbContext>>();
            services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
        }

        //services.AddPooledDbContextFactory<TDbContext>(CreateDbContextOptionsBuilder(
        //    services,
        //    connectionString,
        //    postgresOptions,
        //    configureOptions),
        //    postgresOptions.PoolOptions.Value.MaxPoolSize);

        services.AddDbContextPool<TDbContext>(CreateDbContextOptionsBuilder(
            services,
            connectionString,
            postgresOptions,
            configureOptions),
            postgresOptions.PoolOptions.Value.MaxPoolSize);

        return services;
    }

    public static IServiceCollection ConfigurePostgresSqlTelemetry(
        this IServiceCollection services) =>
            services.ConfigureOpenTelemetryTracerProvider(configure =>
            {
                configure.AddEntityFrameworkCoreInstrumentation(cnf =>
                {
                    cnf.SetDbStatementForText = true;
                    cnf.SetDbStatementForStoredProcedure = true;
                });
                configure.AddNpgsql();
            });


    private static Action<IServiceProvider, DbContextOptionsBuilder> CreateDbContextOptionsBuilder(
        IServiceCollection services,
        string connectionString,
        PostgresDbContextOptions postgresOptions,
        Action<IServiceProvider, DbContextOptionsBuilder>? configureOptions = null)
    {
        if (postgresOptions.EnableAuditableInterceptor)
            services.AddSingleton<AuditableInterceptor>();

        if (postgresOptions.EnablePublishDomainEventsInterceptor)
            services.AddSingleton<PublishDomainEventsInterceptor>();

        return (serviceProvider, options) =>
        {
            options
              .UseNpgsql(connectionString, npgsqlOptions =>
              {
                  postgresOptions.MigrationsAssembly.Execute(migrationsAssembly =>
                      npgsqlOptions.MigrationsAssembly(migrationsAssembly));

                  postgresOptions.RetryOptions.Execute(retryOptions =>
                    npgsqlOptions.EnableRetryOnFailure(retryOptions.MaxRetryCount, retryOptions.MaxRetryDelay, retryOptions.AdditionalErrorCodes));

                  npgsqlOptions.UseQuerySplittingBehavior(postgresOptions.QuerySplittingBehavior);

              })
              .EnableDetailedErrors(postgresOptions.EnableDetailedErrors)
              .EnableSensitiveDataLogging(postgresOptions.EnableSensitiveDataLogging)
              .UseAllCheckConstraints()
              .UseSnakeCaseNamingConvention()
              .UseExceptionProcessor();

            if (postgresOptions.EnableAuditableInterceptor)
                options.AddInterceptors(serviceProvider.GetRequiredService<AuditableInterceptor>());

            if (postgresOptions.EnablePublishDomainEventsInterceptor)
                options.AddInterceptors(serviceProvider.GetRequiredService<PublishDomainEventsInterceptor>());

            options.UseQueryTrackingBehavior(postgresOptions.QueryTrackingBehavior);

            configureOptions?.Invoke(serviceProvider, options);
        };
    }
}

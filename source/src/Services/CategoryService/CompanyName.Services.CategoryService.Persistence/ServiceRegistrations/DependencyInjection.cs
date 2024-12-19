﻿using CSharpEssentials.EntityFrameworkCore.Interceptors;
using MassTransit;
using CompanyName.BuildingBlocks.BackgroundJobs.Quartz;
using CompanyName.BuildingBlocks.Caching.Redis;
using CompanyName.BuildingBlocks.Database.PostgreSQL;
using CompanyName.BuildingBlocks.MessageBrokers.MassTransit.EntityFrameworkCore;
using CompanyName.Services.CategoryService.Persistence.EntityFrameworkCore.Contexts;
using CompanyName.Services.Info;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace CompanyName.Services.CategoryService.Persistence.ServiceRegistrations;
public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IHostEnvironment environment,
        IConfiguration configuration)
    {
        return services
            .ConfigureCacheServices(environment, configuration)
            .AddQuartzWithHostedService()
            .ConfigureJobOptions()
            .AddDatabaseServices(configuration)
            .AddEventBus<ApplicationWriteDbContext>(
                configure => configure.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host(configuration.GetConnectionString(ServiceKeys.RabbitMQ));
                        cfg.ConfigureEndpoints(context);
                    }),
                entityConfigure => entityConfigure.UsePostgres().UseBusOutbox(),
                assemblies: PersistenceAssemblyReference.Assembly)
            .RegisterRepositories()
            .RegisterHttpServices()
            .RegisterServices();
    }
    private static IServiceCollection ConfigureCacheServices(
        this IServiceCollection services,
        IHostEnvironment environment,
        IConfiguration configuration)
    {
        return services
            .AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(sp =>
            {
                string connectionString = configuration.GetConnectionString(ServiceKeys.Redis);
                var configurationOptions = ConfigurationOptions.Parse(connectionString!, true);
                return ConnectionMultiplexer.Connect(configurationOptions);
            })
            .AddCacheServices(
                environment.ApplicationName,
                redisConfigurations => redisConfigurations.Configuration = configuration.GetConnectionString(ServiceKeys.Redis),
                memoryConfiguration =>
                {
                });
    }
    private static IServiceCollection AddDatabaseServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string migrationsAssembly = typeof(PersistenceAssemblyReference).Namespace;
        return services
            .AddSingleton<SlowQueryInterceptor>()
            .AddPooledPostgresDbContext<ApplicationWriteDbContext>(
                configuration.GetConnectionString(ServiceKeys.Database.PostgresCategoryService)!,
                options => options.MigrationsAssembly = migrationsAssembly)
            .AddPooledPostgresDbContext<ApplicationReadDbContext>(
                configuration.GetConnectionString(ServiceKeys.Database.PostgresCategoryService)!,
                options =>
                {
                    options.MigrationsAssembly = migrationsAssembly;
                    options.EnablePublishDomainEventsInterceptor = false;
                    options.EnableAuditableInterceptor = false;
                    options.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                });
    }
}
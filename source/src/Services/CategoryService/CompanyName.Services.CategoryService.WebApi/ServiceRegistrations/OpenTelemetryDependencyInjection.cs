using CompanyName.BuildingBlocks.Caching.Redis;
using CompanyName.BuildingBlocks.Database.PostgreSQL;
using CompanyName.BuildingBlocks.OpenTelemetry.Base;

namespace CompanyName.Services.CategoryService.WebApi.ServiceRegistrations;

internal static class OpenTelemetryDependencyInjection
{
    internal static IServiceCollection ConfigureTelemetries(
        this IServiceCollection services,
        IHostEnvironment hostEnvironment)
    {
        services
           .ConfigureOpenTelemetry(hostEnvironment.ApplicationName)
           .ConfigurePostgresSqlTelemetry()
           .ConfigureCacheServiceTelemetry()
           .AddOtlpExporter();
        return services;
    }
}

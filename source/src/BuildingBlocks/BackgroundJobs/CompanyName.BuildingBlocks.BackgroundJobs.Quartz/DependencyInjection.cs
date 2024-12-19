using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace CompanyName.BuildingBlocks.BackgroundJobs.Quartz;
public static class DependencyInjection
{
    public static IServiceCollection AddQuartzWithHostedService(
        this IServiceCollection services,
        Action<IServiceCollectionQuartzConfigurator>? configurator = null,
        Action<QuartzHostedServiceOptions>? options = null)
    {
        services.AddQuartz(configurator);

        services.AddQuartzHostedService(configure =>
        {
            configure.WaitForJobsToComplete = true;
            options?.Invoke(configure);
        });
        return services;
    }
}

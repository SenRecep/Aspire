using CSharpEssentials.Time;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.BuildingBlocks.Application.DependencyInjections;
public static class DateTimeProviderDependencyInjection
{
    public static IServiceCollection AddDateTimeProvider(
        this IServiceCollection services) => services
            .AddSingleton(TimeProvider.System)
            .AddSingleton<IDateTimeProvider, DateTimeProvider>();
}

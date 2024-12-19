using MassTransit;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace CompanyName.BuildingBlocks.MessageBrokers.MassTransit.EntityFrameworkCore;
public static class DependencyInjection
{
    public static IServiceCollection AddEventBus<TDbContext>(
        this IServiceCollection services,
        Action<IBusRegistrationConfigurator>? configureAction = null,
        Action<IEntityFrameworkOutboxConfigurator>? entityFrameworkOutboxConfigurator = null,
        bool consumeObserver = true,
        bool publishObserver = true,
        params Assembly[] assemblies)
        where TDbContext : DbContext
    {
        return services.AddEventBus(configure =>
        {
            configure.AddEntityFrameworkOutbox<TDbContext>(entityConfigure =>
            {
                entityConfigure.DuplicateDetectionWindow = TimeSpan.FromSeconds(30);
                entityFrameworkOutboxConfigurator?.Invoke(entityConfigure);
            });
            configureAction?.Invoke(configure);
        },
        consumeObserver: consumeObserver,
        publishObserver: publishObserver,
        isOutboxEnabled: true,
        assemblies: assemblies);
    }
}

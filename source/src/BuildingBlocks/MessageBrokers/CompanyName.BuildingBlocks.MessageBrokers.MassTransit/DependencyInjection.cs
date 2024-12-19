using System.Reflection;
using MassTransit;
using CompanyName.BuildingBlocks.MessageBrokers.Base;
using CompanyName.BuildingBlocks.MessageBrokers.MassTransit.Observers;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Trace;

namespace CompanyName.BuildingBlocks.MessageBrokers.MassTransit;
public static class DependencyInjection
{
    public static IServiceCollection AddEventBus(
        this IServiceCollection services,
        Action<IBusRegistrationConfigurator>? configureAction = null,
        bool consumeObserver = true,
        bool publishObserver = true,
        bool isOutboxEnabled = false,
        params Assembly[] assemblies)
    {
        if (assemblies.Length == 0)
            assemblies = [Assembly.GetCallingAssembly()];
        services.AddTransient<IEventBus>(sp =>
        {
            IBus bus = sp.GetRequiredService<IBus>();
            IPublishEndpoint publishEndpoint = sp.GetRequiredService<IPublishEndpoint>();
            return new EventBus(publishEndpoint, bus, isOutboxEnabled);
        });

        services.AddMassTransit(configure =>
        {
            configure.SetKebabCaseEndpointNameFormatter();
            configure.AddConsumers(assemblies);
            configure.AddActivities(assemblies);

            if (consumeObserver)
                configure.AddConsumeObserver<ConsumeLoggerObserver>();
            if (publishObserver)
                configure.AddPublishObserver<PublishLoggerObserver>();

            configureAction?.Invoke(configure);
        });

        return services;
    }

    public static IServiceCollection ConfigureEventBusTelemetry(
        this IServiceCollection services) =>
        services
        .ConfigureOpenTelemetryTracerProvider(configure =>
            configure.AddSource(global::MassTransit.Logging.DiagnosticHeaders.DefaultListenerName));
}

using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using OpenTelemetry;
using OpenTelemetry.Resources;

namespace CompanyName.BuildingBlocks.OpenTelemetry.Base;
public static class DependencyInjection
{
    public static IServiceCollection ConfigureOpenTelemetry(
       this IServiceCollection services,
       string serviceName,
       string? serviceNamespace = nameof(CompanyName),
       Action<OpenTelemetryBuilder>? configure = null,
       Action<OpenTelemetryLoggerOptions>? configureLoggerOptions = null,
       Action<MeterProviderBuilder>? configureMeter = null,
       Action<TracerProviderBuilder>? configureTrace = null)
    {
        OpenTelemetryBuilder openTelemetryBuilder = services.AddOpenTelemetry();

        openTelemetryBuilder.ConfigureResource(resource =>
        {
            resource.AddService(serviceName, serviceNamespace);
            resource.AddTelemetrySdk();
        });

        configure?.Invoke(openTelemetryBuilder);

        services.Configure<OpenTelemetryLoggerOptions>(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
            logging.ParseStateValues = true;
            configureLoggerOptions?.Invoke(logging);
        })
        .ConfigureOpenTelemetryMeterProvider(metrics =>
        {
            metrics
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddProcessInstrumentation()
                .AddRuntimeInstrumentation()
                .AddEventCountersInstrumentation();

            configureMeter?.Invoke(metrics);
        })
        .ConfigureOpenTelemetryTracerProvider(tracing =>
        {
            tracing
            .SetErrorStatusOnException()
            .AddAspNetCoreInstrumentation(options =>
            {
                options.RecordException = true;
                options.EnrichWithException = (activity, exception) =>
                {
                    activity.SetTag("exceptionType", exception.GetType().ToString());
                    activity.SetTag("stackTrace", exception.StackTrace);
                };
            })
            .AddHttpClientInstrumentation(options => options.RecordException = true)
            .AddGrpcClientInstrumentation();

            configureTrace?.Invoke(tracing);
        });

        services.AddMetrics();
        services.AddOpenTelemetry();

        return services;
    }

    public static IOpenTelemetryBuilder AddOtlpExporter(
       this IServiceCollection services)
    {
        return services
            .AddOpenTelemetry()
            .UseOtlpExporter();
    }
}

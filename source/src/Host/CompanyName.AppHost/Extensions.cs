using HealthChecks.NpgSql;
using HealthChecks.RabbitMQ;
using HealthChecks.Redis;
using HealthChecks.Uris;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CompanyName.AppHost;
public static class Extensions
{
    public static IResourceBuilder<RabbitMQServerResource> WithHealthCheck(this IResourceBuilder<RabbitMQServerResource> builder)
    {
        return builder.WithAnnotation(HealthCheckAnnotation.Create(cs => new RabbitMQHealthCheck(new RabbitMQHealthCheckOptions { ConnectionUri = new(cs) })));
    }

    public static IResourceBuilder<RedisResource> WithHealthCheck(this IResourceBuilder<RedisResource> builder)
    {
        return builder.WithAnnotation(HealthCheckAnnotation.Create(cs => new RedisHealthCheck(cs)));
    }

    public static IResourceBuilder<PostgresServerResource> WithHealthCheck(this IResourceBuilder<PostgresServerResource> builder)
    {
        return builder.WithAnnotation(HealthCheckAnnotation.Create(cs => new NpgSqlHealthCheck(new NpgSqlHealthCheckOptions(cs))));
    }

    public static IResourceBuilder<T> WithHealthCheck<T>(
        this IResourceBuilder<T> builder,
        string? endpointName = null,
        string path = "health",
        Action<UriHealthCheckOptions>? configure = null)
        where T : IResourceWithEndpoints
    {
        return builder.WithAnnotation(new HealthCheckAnnotation((resource, ct) =>
        {
            if (resource is not IResourceWithEndpoints resourceWithEndpoints)
            {
                return Task.FromResult<IHealthCheck?>(null);
            }

            EndpointReference? endpoint = endpointName is null
             ? resourceWithEndpoints.GetEndpoints().FirstOrDefault(e => e.Scheme is "http" or "https")
             : resourceWithEndpoints.GetEndpoint(endpointName);

            string? url = endpoint?.Url;

            if (url is null)
            {
                return Task.FromResult<IHealthCheck?>(null);
            }

            var options = new UriHealthCheckOptions();

            options.AddUri(new(new(url), path));

            configure?.Invoke(options);

            var client = new HttpClient();
            return Task.FromResult<IHealthCheck?>(new UriHealthCheck(options, () => client));
        }));
    }
}

public class HealthCheckAnnotation(Func<IResource, CancellationToken, Task<IHealthCheck?>> healthCheckFactory) : IResourceAnnotation
{
    public Func<IResource, CancellationToken, Task<IHealthCheck?>> HealthCheckFactory { get; } = healthCheckFactory;

    public static HealthCheckAnnotation Create(Func<string, IHealthCheck> connectionStringFactory)
    {
        return new(async (resource, token) =>
        {
            if (resource is not IResourceWithConnectionString c)
            {
                return null;
            }

            if (await c.GetConnectionStringAsync(token) is not string cs)
            {
                return null;
            }

            return connectionStringFactory(cs);
        });
    }
}

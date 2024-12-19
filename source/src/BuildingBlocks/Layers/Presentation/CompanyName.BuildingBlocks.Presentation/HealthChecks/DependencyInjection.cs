using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.BuildingBlocks.Presentation.HealthChecks;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureHttpClients(
        this IServiceCollection services)
    {
        return services
            .AddHttpClient()
            .AddServiceDiscovery()
            .ConfigureHttpClientDefaults(http =>
            {
                http.AddStandardResilienceHandler();
                http.AddServiceDiscovery();
            });
    }

    public static IApplicationBuilder UseDefaultHealthChecks(this IApplicationBuilder app, string path = "/health") =>
        app.UseHealthChecks(path, new HealthCheckOptions()
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
}

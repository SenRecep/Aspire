using CompanyName.BuildingBlocks.Persistence.HttpHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.Services.CategoryService.Persistence.ServiceRegistrations;
internal static class HttpServicesRegistrations
{
    public static IServiceCollection RegisterHttpServices(this IServiceCollection services)
    {
        services.AddTransient<AuthHeaderHandler>();

        return services;
    }
}

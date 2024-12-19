using CompanyName.BuildingBlocks.Persistence.Extensions;
using CompanyName.BuildingBlocks.Persistence.HttpHandlers;
using CompanyName.Services.Info;
using CompanyName.Services.ProductService.Persistence.Categories.HttpServices;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace CompanyName.Services.ProductService.Persistence.ServiceRegistrations;
internal static class HttpServicesRegistrations
{
    public static IServiceCollection RegisterHttpServices(this IServiceCollection services)
    {
        services.AddTransient<AuthHeaderHandler>();
        services
            .AddRefitClient<IHttpCategoryService>()
            .ConfigureHttpClientWithServiceName(ServiceKeys.CategoryService)
            .AddHttpMessageHandler<AuthHeaderHandler>();


        return services;
    }
}

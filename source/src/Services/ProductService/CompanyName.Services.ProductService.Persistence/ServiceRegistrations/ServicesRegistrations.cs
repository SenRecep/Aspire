using CompanyName.Services.ProductService.Domain.Categories.Services;
using CompanyName.Services.ProductService.Persistence.Categories.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.Services.ProductService.Persistence.ServiceRegistrations;

internal static class ServicesRegistrations
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        return services;
    }
}

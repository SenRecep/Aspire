using CompanyName.Services.ProductService.Domain.Categories.Repositories;
using CompanyName.Services.ProductService.Domain.Products.Repositories;
using CompanyName.Services.ProductService.Persistence.EntityFrameworkCore.Repositories.Categories;
using CompanyName.Services.ProductService.Persistence.EntityFrameworkCore.Repositories.Products;

using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.Services.ProductService.Persistence.ServiceRegistrations;
internal static class RepositoryRegistrations
{
    public static IServiceCollection RegisterRepositories(this IServiceCollection services) =>
        services
            .AddScoped<ICategoryCommandRepository, EfCategoryCommandRepository>()
            .AddScoped<IProductCommandRepository, EfProductCommandRepository>()
            .AddScoped<IProductQueryRepository, EfProductQueryRepository>();
}

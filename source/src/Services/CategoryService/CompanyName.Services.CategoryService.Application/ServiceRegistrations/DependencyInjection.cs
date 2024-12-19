using FluentValidation;
using CompanyName.BuildingBlocks.Application.DependencyInjections;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.Services.CategoryService.Application.ServiceRegistrations;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        return services
            .AddDateTimeProvider()
            .AddMediatR(configure =>
            {
                configure.RegisterServicesFromAssembly(ApplicationAssemblyReference.Assembly);
                configure.AddDefaultBehaviors();
            })
            .AddValidatorsFromAssembly(ApplicationAssemblyReference.Assembly, includeInternalTypes: true);
    }
}

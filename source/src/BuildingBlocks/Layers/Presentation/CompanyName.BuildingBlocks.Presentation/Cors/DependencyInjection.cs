using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.BuildingBlocks.Presentation.Cors;
public static class DependencyInjection
{
    public static IServiceCollection AddAllAcceptCors(this IServiceCollection services, string policyName = "all")
    {
        return services.AddCors(options =>
             options.AddPolicy(policyName, builder =>
                 builder.AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader()
             )
         );
    }

}

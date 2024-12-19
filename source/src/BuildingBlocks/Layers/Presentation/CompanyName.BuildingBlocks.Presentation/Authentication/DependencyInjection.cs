using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.BuildingBlocks.Presentation.Authentication;
public static class DependencyInjection
{
    public static IServiceCollection AddKeycloakJwtBearer(
        this IServiceCollection services,
        string keycloakServiceId = "keycloak",
        string realm = "CompanyName")
    {
        services
           .AddAuthorization()
           .AddAuthentication()
            .AddKeycloakJwtBearer(keycloakServiceId, realm, options =>
            {
                options.RequireHttpsMetadata = false;
                options.Audience = "account";
            });

        return services;
    }
}

using System.Reflection;
using Carter;
using CSharpEssentials;
using CSharpEssentials.AspNetCore;
using CSharpEssentials.RequestResponseLogging;
using CompanyName.BuildingBlocks.Application.Shared.Constants;
using CompanyName.BuildingBlocks.Presentation.Authentication;
using CompanyName.BuildingBlocks.Presentation.Cors;
using CompanyName.BuildingBlocks.Presentation.HealthChecks;
using CompanyName.BuildingBlocks.Presentation.SessionContexts;
using CompanyName.Services.Info;
using CompanyName.Services.ProductService.Application.ServiceRegistrations;
using CompanyName.Services.ProductService.Persistence.ServiceRegistrations;
using Microsoft.Net.Http.Headers;

namespace CompanyName.Services.ProductService.WebApi.ServiceRegistrations;

internal static class DependencyInjection
{
    internal static IServiceCollection AddServiceRegistrations(
        this IServiceCollection services,
        IHostEnvironment hostEnvironment,
        IConfiguration configuration)
    {
        //services.AddControllers();
        //services.AddRouting();
        services.AddCarter();
        services.AddHealthChecks();

        return services
            .AddAllAcceptCors()
            .AddHttpContextAccessor()
            .AddApplicationServices()
            .AddPersistenceServices(hostEnvironment, configuration)
            .AddSessionContext()
            .ConfigureHttpClients()
            .AddExceptionHandler<GlobalExceptionHandler>()
            .ConfigureModelValidatorResponse()
            .ConfigureSystemTextJson()
            .AddEnhancedProblemDetails()
            .AddAndConfigureApiVersioning()
            .AddSwagger<DefaultConfigureSwaggerOptions>(SecuritySchemes.JwtBearerTokenSecurity, Assembly.GetExecutingAssembly())
            .AddKeycloakJwtBearer(keycloakServiceId: ServiceKeys.Keycloak, realm: "products")
            .ConfigureTelemetries(hostEnvironment);
    }

    internal static WebApplication UseServices(
        this WebApplication app)
    {
        app.UseVersionableSwagger();
        app.AddRequestResponseLogging(opt =>
        {
            opt.IgnorePaths("/health");
            var loggingOptions = LoggingOptions.CreateAllFields();
            loggingOptions.HeaderKeys.Add(HeaderNames.AcceptLanguage);
            loggingOptions.HeaderKeys.Add(CustomHeaderNames.TenantId);
            opt.UseLogger(app.Services.GetRequiredService<ILoggerFactory>(), loggingOptions);
        });
        app.UseExceptionHandler();
        app.UseStatusCodePages();
        app.UseCors("all");
        //app.UseRouting();
        //app.MapControllers();
        app.MapCarter();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseDefaultHealthChecks();

        return app;
    }
}

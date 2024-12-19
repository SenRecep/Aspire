using Asp.Versioning.Builder;
using Asp.Versioning;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CompanyName.BuildingBlocks.Presentation.Endpoints;
public static class EndpointExtensions
{
    public static ApiVersionSet CreateVersionSet(this IEndpointRouteBuilder app, int version = 1)
    {
        return app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(version))
            .ReportApiVersions()
            .Build();
    }
    public static RouteGroupBuilder CreateVersionedGroup(this IEndpointRouteBuilder app, string route, int version = 1)
    {
        ApiVersionSet apiVersionSet = app.CreateVersionSet(version);

        RouteGroupBuilder versionedGroup = app
            .MapGroup($"v{{version:apiVersion}}/{route}")
            .WithApiVersionSet(apiVersionSet)
            .WithTags(route);

        return versionedGroup;

    }
}

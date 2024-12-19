using Carter;
using CSharpEssentials;
using MediatR;
using CompanyName.BuildingBlocks.Presentation.Endpoints;
using CompanyName.Services.CategoryService.Application.Categories.v1.Commands.Create;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.Services.CategoryService.WebApi.Endpoints.Categories.v1.Create;
public sealed class CategoryCreateEndpoint : CarterModule
{
    public sealed record CategoryCreateRequest(string? Name)
    {
        public CategoryCreateCommand ToCommand() => new(Name);
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder routeGroup = app
            .CreateVersionedGroup(Tags.Categories)
            .RequireAuthorization();

        routeGroup.MapPost(string.Empty, CreateCategory)
            .Produces<Guid>(HttpCodes.Created)
            .ProducesProblem()
            .WithDescription("Create Category")
            .WithName(nameof(CreateCategory));
    }

    private static Task<IResult> CreateCategory([FromBody] CategoryCreateRequest request, ISender sender, CancellationToken cancellationToken = default) =>
            sender
               .Send(request.ToCommand(), cancellationToken)
               .Match(
                     category => TypedResults.Created($"/{Tags.Categories}/{category.Value}", category.Value),
                     errors => errors.ToProblemResult(),
                     cancellationToken);
}

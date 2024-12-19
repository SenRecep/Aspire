using Carter;
using CSharpEssentials;
using MediatR;
using CompanyName.BuildingBlocks.Presentation.Endpoints;
using CompanyName.Services.CategoryService.Application.Categories.v1.Queries.Exist;

namespace CompanyName.Services.CategoryService.WebApi.Endpoints.Categories.v1.Exist;

public sealed class CategoryExistEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder routeGroup = app
            .CreateVersionedGroup(Tags.Exist)
            .RequireAuthorization();

        routeGroup.MapGet("{categoryId:guid}", ExistCategory)
            .Produces<bool>()
            .ProducesProblem()
            .WithDescription("Check if Category exists by id")
            .WithName(nameof(ExistCategory));
    }

    private static Task<IResult> ExistCategory(Guid categoryId, ISender sender, CancellationToken cancellationToken = default) =>
            sender
               .Send(new CategoryExistQuery(categoryId), cancellationToken)
               .Match(
                     exist => TypedResults.Ok(exist),
                     errors => errors.ToProblemResult(),
                     cancellationToken);
}

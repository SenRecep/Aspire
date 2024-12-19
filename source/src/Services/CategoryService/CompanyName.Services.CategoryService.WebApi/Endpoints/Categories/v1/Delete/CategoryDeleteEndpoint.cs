using Carter;
using CSharpEssentials;
using MediatR;
using CompanyName.BuildingBlocks.Presentation.Endpoints;
using CompanyName.Services.CategoryService.Application.Categories.v1.Commands.Delete;

namespace CompanyName.Services.CategoryService.WebApi.Endpoints.Categories.v1.Delete;

public sealed class CategoryDeleteEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder routeGroup = app
            .CreateVersionedGroup(Tags.Categories)
            .RequireAuthorization();

        routeGroup.MapDelete("{categoryId:guid}", DeleteCategory)
            .Produces(HttpCodes.NoContent)
            .ProducesProblem()
            .WithDescription("Delete Category by id")
            .WithName(nameof(DeleteCategory));
    }

    private static Task<IResult> DeleteCategory(Guid categoryId, ISender sender, CancellationToken cancellationToken = default) =>
            sender
               .Send(new CategoryDeleteCommand(categoryId), cancellationToken)
               .Match(
                     TypedResults.NoContent,
                     errors => errors.ToProblemResult(),
                     cancellationToken);
}

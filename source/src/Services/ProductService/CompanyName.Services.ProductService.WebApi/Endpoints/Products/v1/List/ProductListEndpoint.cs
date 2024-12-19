using Carter;
using CSharpEssentials;
using MediatR;
using CompanyName.BuildingBlocks.Presentation.Endpoints;
using CompanyName.Services.ProductService.Application.Products.v1.Models;
using CompanyName.Services.ProductService.Application.Products.v1.Queries.List;

namespace CompanyName.Services.ProductService.WebApi.Endpoints.Products.v1.List;

public sealed class ProductListEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder routeGroup = app
            .CreateVersionedGroup(Tags.Products)
            .RequireAuthorization();

        routeGroup.MapGet("category/{categoryId:guid}", GetProductsByCategory)
        .Produces<ProductViewModel[]>()
        .ProducesProblem()
        .WithDescription("Get products by category")
        .WithName(nameof(GetProductsByCategory));
    }

    private static Task<IResult> GetProductsByCategory(Guid categoryId, ISender sender, CancellationToken cancellationToken = default) =>
            sender
               .Send(new GetProductListQuery(categoryId), cancellationToken)
               .Match(
                     products => TypedResults.Ok(products),
                     errors => errors.ToProblemResult(),
                     cancellationToken);
}

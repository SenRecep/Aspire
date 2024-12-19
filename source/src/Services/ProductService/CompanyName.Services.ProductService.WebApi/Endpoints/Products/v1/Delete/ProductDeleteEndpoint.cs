using Carter;
using CSharpEssentials;
using MediatR;
using CompanyName.BuildingBlocks.Presentation.Endpoints;
using CompanyName.Services.ProductService.Application.Products.v1.Commands.Delete;

namespace CompanyName.Services.ProductService.WebApi.Endpoints.Products.v1.Delete;

public sealed class ProductDeleteEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder routeGroup = app
            .CreateVersionedGroup(Tags.Products)
            .RequireAuthorization();

        routeGroup.MapDelete("{productId:guid}", DeleteProduct)
            .Produces(HttpCodes.NoContent)
            .ProducesProblem()
            .WithDescription("Delete product by id")
            .WithName(nameof(DeleteProduct));
    }

    private static Task<IResult> DeleteProduct(Guid productId, ISender sender, CancellationToken cancellationToken = default) =>
            sender
               .Send(new ProductDeleteCommand(productId), cancellationToken)
               .Match(
                     TypedResults.NoContent,
                     errors => errors.ToProblemResult(),
                     cancellationToken);
}

using Carter;
using CSharpEssentials;
using MediatR;
using CompanyName.BuildingBlocks.Presentation.Endpoints;
using CompanyName.Services.ProductService.Application.Products.v1.Commands.Create;
using CompanyName.Services.ProductService.Domain.Products.Fields;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.Services.ProductService.WebApi.Endpoints.Products.v1.Create;
public sealed class ProductCreateEndpoint : CarterModule
{
    public sealed record ProductCreateRequest(string? Name, string? Description, decimal Price, Currency Currency, Guid Category)
    {
        public ProductCreateCommand ToCommand() =>
            new(Name, Description, Price, Currency, Category);
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder routeGroup = app
            .CreateVersionedGroup(Tags.Products)
            .RequireAuthorization();

        routeGroup.MapPost(string.Empty, CreateProduct)
            .Produces<Guid>(HttpCodes.Created)
            .ProducesProblem()
            .WithDescription("Create product")
            .WithName(nameof(CreateProduct));
    }

    private static Task<IResult> CreateProduct([FromBody] ProductCreateRequest request, ISender sender, CancellationToken cancellationToken = default) =>
            sender
               .Send(request.ToCommand(), cancellationToken)
               .Match(
                     product => TypedResults.Created($"/{Tags.Products}/{product.Value}", product.Value),
                     errors => errors.ToProblemResult(),
                     cancellationToken);
}

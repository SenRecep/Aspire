using CSharpEssentials;
using CompanyName.BuildingBlocks.Application.Abstractions.Contracts;
using CompanyName.Services.ProductService.Domain.Products.Fields;
using CompanyName.Services.ProductService.Domain.Products.Repositories;

namespace CompanyName.Services.ProductService.Application.Products.v1.Commands.Delete;

internal sealed class ProductDeleteCommandHandler(
    IProductCommandRepository repository) : ICommandHandler<ProductDeleteCommand>
{
    public Task<Result> Handle(ProductDeleteCommand request, CancellationToken cancellationToken)
    {
        var productId = ProductId.From(request.ProductId);
        return repository.DeleteProductAsync(productId, cancellationToken);
    }
}

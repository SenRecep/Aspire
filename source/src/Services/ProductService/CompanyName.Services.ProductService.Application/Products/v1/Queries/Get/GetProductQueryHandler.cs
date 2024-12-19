using CSharpEssentials;
using CompanyName.BuildingBlocks.Application.Abstractions.Contracts;
using CompanyName.Services.ProductService.Application.Products.v1.Models;
using CompanyName.Services.ProductService.Domain.Products;
using CompanyName.Services.ProductService.Domain.Products.Fields;
using CompanyName.Services.ProductService.Domain.Products.ReadModels;
using CompanyName.Services.ProductService.Domain.Products.Repositories;

namespace CompanyName.Services.ProductService.Application.Products.v1.Queries.Get;

internal sealed class GetProductQueryHandler(
    IProductQueryRepository productQueryRepository) : ICachedQueryHandler<GetProductQuery, ProductViewModel>
{
    public async Task<Result<ProductViewModel>> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var productId = ProductId.From(request.ProductId);
        Maybe<ProductReadModel> product = await productQueryRepository.GetProductByIdAsync(productId, cancellationToken);

        return product.Match<Result<ProductViewModel>>(
            value => ProductViewModel.Create(value),
            () => ProductErrors.ProductDoesNotExistError(productId));
    }
}

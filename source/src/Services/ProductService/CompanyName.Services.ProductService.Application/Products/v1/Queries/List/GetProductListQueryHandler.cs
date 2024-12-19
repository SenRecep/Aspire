using CSharpEssentials;
using CompanyName.BuildingBlocks.Application.Abstractions.Contracts;
using CompanyName.Services.ProductService.Application.Products.v1.Models;
using CompanyName.Services.ProductService.Domain.Products.Fields;
using CompanyName.Services.ProductService.Domain.Products.ReadModels;
using CompanyName.Services.ProductService.Domain.Products.Repositories;

namespace CompanyName.Services.ProductService.Application.Products.v1.Queries.List;

internal sealed class GetProductListQueryHandler(
    IProductQueryRepository repository) : ICachedQueryHandler<GetProductListQuery, ProductViewModel[]>
{
    public async Task<Result<ProductViewModel[]>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
    {
        var categoryId = CategoryId.From(request.CategoryId);
        ProductReadModel[] products = await repository.GetProductsByCategoryId(categoryId, cancellationToken);
        ProductViewModel[] models = ProductViewModel.Create(products);
        return models;

    }
}

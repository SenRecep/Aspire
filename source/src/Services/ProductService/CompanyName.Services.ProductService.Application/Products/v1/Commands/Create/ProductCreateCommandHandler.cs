using CSharpEssentials;
using CompanyName.BuildingBlocks.Application.Abstractions.Contracts;
using CompanyName.Services.ProductService.Domain.Categories.Rules.Exist;
using CompanyName.Services.ProductService.Domain.Categories.Services;
using CompanyName.Services.ProductService.Domain.Products.Fields;
using CompanyName.Services.ProductService.Domain.Products.Parameters;
using CompanyName.Services.ProductService.Domain.Products.Repositories;

namespace CompanyName.Services.ProductService.Application.Products.v1.Commands.Create;

internal sealed class ProductCreateCommandHandler(
    IProductCommandRepository repository,
    ICategoryService categoryService) : ICommandHandler<ProductCreateCommand, ProductId>
{
    public Task<Result<ProductId>> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
    {
        var rule = new CategoryExistRule(categoryService);
        ProductCreateParameters parameters = request.ToParameters();
        return repository.CreateProductAsync(parameters, rule, cancellationToken);
    }
}

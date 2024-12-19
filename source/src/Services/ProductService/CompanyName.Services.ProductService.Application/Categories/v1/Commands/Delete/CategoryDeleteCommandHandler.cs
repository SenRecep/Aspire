using CSharpEssentials;
using CompanyName.BuildingBlocks.Application.Abstractions.Contracts;
using CompanyName.BuildingBlocks.Caching.Base;
using CompanyName.Services.ProductService.Domain.Categories.Repositories;
using CompanyName.Services.ProductService.Domain.Products.Fields;

namespace CompanyName.Services.ProductService.Application.Categories.v1.Commands.Delete;

internal sealed class CategoryDeleteCommandHandler
    (ICategoryCommandRepository repository,
    ICacheService cacheService) : ICommandHandler<CategoryDeleteCommand>
{
    public async Task<Result> Handle(CategoryDeleteCommand request, CancellationToken cancellationToken)
    {
        var categoryId = CategoryId.From(request.CategoryId);
        Result result = await repository.DeleteProductsByCategoryIdAsync(categoryId, cancellationToken);
        if (result.IsFailure)
            return result;

        string key = $"category:{request.CategoryId}:products";
        cacheService.Remove(key);

        return result;
    }
}

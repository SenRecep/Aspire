using CSharpEssentials;
using CompanyName.BuildingBlocks.Application.Abstractions.Contracts;
using CompanyName.Services.CategoryService.Domain.Categories.Fields;
using CompanyName.Services.CategoryService.Domain.Categories.Repositories;

namespace CompanyName.Services.CategoryService.Application.Categories.v1.Commands.Delete;

internal sealed class CategoryDeleteCommandHandler(
    ICategoryCommandRepository repository) : ICommandHandler<CategoryDeleteCommand>
{
    public Task<Result> Handle(CategoryDeleteCommand request, CancellationToken cancellationToken)
    {
        var productId = CategoryId.From(request.CategoryId);
        return repository.DeleteCategoryAsync(productId, cancellationToken);
    }
}

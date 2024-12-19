using CSharpEssentials;
using CompanyName.BuildingBlocks.Application.Abstractions.Contracts;
using CompanyName.Services.CategoryService.Domain.Categories.Fields;
using CompanyName.Services.CategoryService.Domain.Categories.Parameters;
using CompanyName.Services.CategoryService.Domain.Categories.Repositories;
namespace CompanyName.Services.CategoryService.Application.Categories.v1.Commands.Create;

internal sealed class CategoryCreateCommandHandler(
    ICategoryCommandRepository repository) : ICommandHandler<CategoryCreateCommand, CategoryId>
{
    public Task<Result<CategoryId>> Handle(CategoryCreateCommand request, CancellationToken cancellationToken)
    {
        CategoryCreateParameters parameters = request.ToParameters();
        return repository.CreateCategoryAsync(parameters, cancellationToken);
    }
}

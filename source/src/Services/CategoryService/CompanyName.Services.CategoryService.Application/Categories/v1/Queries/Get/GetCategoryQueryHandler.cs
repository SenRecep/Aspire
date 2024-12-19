using CSharpEssentials;
using CompanyName.BuildingBlocks.Application.Abstractions.Contracts;
using CompanyName.Services.CategoryService.Application.Products.v1.Models;
using CompanyName.Services.CategoryService.Domain.Categories;
using CompanyName.Services.CategoryService.Domain.Categories.Fields;
using CompanyName.Services.CategoryService.Domain.Categories.ReadModels;
using CompanyName.Services.CategoryService.Domain.Categories.Repositories;

namespace CompanyName.Services.CategoryService.Application.Categories.v1.Queries.Get;

internal sealed class GetCategoryQueryHandler(
    ICategoryQueryRepository categoryQueryRepository) : ICachedQueryHandler<GetCategoryQuery, CategoryViewModel>
{
    public async Task<Result<CategoryViewModel>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var categoryId = CategoryId.From(request.CategoryId);
        Maybe<CategoryReadModel> Category = await categoryQueryRepository.GetCategoryByIdAsync(categoryId, cancellationToken);

        return Category.Match<Result<CategoryViewModel>>(
            value => CategoryViewModel.Create(value),
            () => CategoryErrors.CategoryDoesNotExistError(categoryId));
    }
}

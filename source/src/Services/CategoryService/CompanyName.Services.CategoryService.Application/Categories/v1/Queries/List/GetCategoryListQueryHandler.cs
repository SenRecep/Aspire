using CSharpEssentials;
using CompanyName.BuildingBlocks.Application.Abstractions.Contracts;
using CompanyName.Services.CategoryService.Application.Products.v1.Models;
using CompanyName.Services.CategoryService.Domain.Categories.ReadModels;
using CompanyName.Services.CategoryService.Domain.Categories.Repositories;

namespace CompanyName.Services.CategoryService.Application.Categories.v1.Queries.List;

internal sealed class GetCategoryListQueryHandler(
    ICategoryQueryRepository repository) : ICachedQueryHandler<GetCategoryListQuery, CategoryViewModel[]>
{
    public async Task<Result<CategoryViewModel[]>> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
    {
        CategoryReadModel[] categories = await repository.GetCategories(cancellationToken);
        CategoryViewModel[] models = CategoryViewModel.Create(categories);
        return models;

    }
}

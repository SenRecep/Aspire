using CSharpEssentials;
using CompanyName.Services.CategoryService.Domain.Categories.Fields;
using CompanyName.Services.CategoryService.Domain.Categories.Parameters;

namespace CompanyName.Services.CategoryService.Domain.Categories.Repositories;
public interface ICategoryCommandRepository
{
    Task<Result<CategoryId>> CreateCategoryAsync(CategoryCreateParameters parameters, CancellationToken cancellationToken = default);
    Task<Result> DeleteCategoryAsync(CategoryId categoryId, CancellationToken cancellationToken = default);
}

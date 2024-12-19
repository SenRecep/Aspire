using CSharpEssentials;
using CompanyName.Services.CategoryService.Domain.Categories.Fields;
using CompanyName.Services.CategoryService.Persistence.EntityFrameworkCore.Contexts;
using CompanyName.Services.CategoryService.Domain.Categories.Repositories;
using CompanyName.Services.CategoryService.Domain.Categories.Parameters;
using CompanyName.Services.CategoryService.Domain.Categories;
using Microsoft.EntityFrameworkCore;

namespace CompanyName.Services.CategoryService.Persistence.EntityFrameworkCore.Repositories.Categories;

internal sealed class EfCategoryCommandRepository(
    ApplicationWriteDbContext context) : ICategoryCommandRepository
{
    public async Task<Result<CategoryId>> CreateCategoryAsync(CategoryCreateParameters parameters, CancellationToken cancellationToken = default)
    {
        Result<Category> categoryResult = Category.Create(parameters);
        if (categoryResult.IsFailure)
            return categoryResult.Errors;

        Category category = categoryResult.Value;

        await context.Categories.AddAsync(category, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return category.Id;
    }

    public async Task<Result> DeleteCategoryAsync(CategoryId categoryId, CancellationToken cancellationToken = default)
    {
        Category? found = await context.Categories
            .Where(Category => Category.Id == categoryId)
            .FirstOrDefaultAsync(cancellationToken);

        if (found is null)
            return CategoryErrors.CategoryDoesNotExistError(categoryId);

        found.Delete();
        context.Categories.Remove(found);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

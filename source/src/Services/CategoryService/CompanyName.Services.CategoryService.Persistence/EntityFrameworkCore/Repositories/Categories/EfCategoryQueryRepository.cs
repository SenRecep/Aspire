using CSharpEssentials;
using CompanyName.Services.CategoryService.Domain.Categories.Fields;
using CompanyName.Services.CategoryService.Domain.Categories.ReadModels;
using CompanyName.Services.CategoryService.Domain.Categories.Repositories;
using CompanyName.Services.CategoryService.Persistence.EntityFrameworkCore.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CompanyName.Services.CategoryService.Persistence.EntityFrameworkCore.Repositories.Categories;
internal sealed class EfCategoryQueryRepository(
    ApplicationReadDbContext context) : ICategoryQueryRepository
{
    public Task<bool> ExistsAsync(CategoryId categoryId, CancellationToken cancellationToken = default)
    {
        return context.Categories
            .AnyAsync(Category => Category.Id == categoryId.Value, cancellationToken);
    }

    public Task<CategoryReadModel[]> GetCategories(CancellationToken cancellationToken = default)
    {
        return context.Categories
            .OrderByDescending(Category => Category.CreatedAt)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<Maybe<CategoryReadModel>> GetCategoryByIdAsync(CategoryId categoryId, CancellationToken cancellationToken = default)
    {
        return await context.Categories
            .Where(Category => Category.Id == categoryId.Value)
            .FirstOrDefaultAsync(cancellationToken);
    }
}

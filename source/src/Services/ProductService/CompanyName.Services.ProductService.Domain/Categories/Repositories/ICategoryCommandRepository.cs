using CSharpEssentials;
using CompanyName.Services.ProductService.Domain.Products.Fields;

namespace CompanyName.Services.ProductService.Domain.Categories.Repositories;
public interface ICategoryCommandRepository
{
    Task<Result> DeleteProductsByCategoryIdAsync(CategoryId categoryId, CancellationToken cancellationToken = default);
}

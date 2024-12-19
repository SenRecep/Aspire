using CSharpEssentials;
using CompanyName.Services.ProductService.Domain.Products.Fields;

namespace CompanyName.Services.ProductService.Domain.Categories.Services;
public interface ICategoryService
{
    Task<Result<bool>> CategoryExistsAsync(CategoryId categoryId, CancellationToken cancellationToken = default);
}

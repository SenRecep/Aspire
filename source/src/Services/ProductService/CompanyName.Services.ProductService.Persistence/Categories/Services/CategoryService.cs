using CSharpEssentials;
using CompanyName.Services.ProductService.Domain.Categories.Services;
using CompanyName.Services.ProductService.Domain.Products.Fields;
using CompanyName.Services.ProductService.Persistence.Categories.HttpServices;

namespace CompanyName.Services.ProductService.Persistence.Categories.Services;
internal sealed class CategoryService(
    IHttpCategoryService httpCategoryService) : ICategoryService
{
    public async Task<Result<bool>> CategoryExistsAsync(CategoryId categoryId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await httpCategoryService.ExistAsync(categoryId.Value, cancellationToken);
        }
        catch (Exception ex)
        {
            return Error.Exception(ex);
        }
    }
}

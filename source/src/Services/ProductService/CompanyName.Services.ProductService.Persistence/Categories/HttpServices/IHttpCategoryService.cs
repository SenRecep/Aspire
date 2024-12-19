using Refit;

namespace CompanyName.Services.ProductService.Persistence.Categories.HttpServices;
public interface IHttpCategoryService
{
    [Get("/v1/exist/{categoryId}")]
    public Task<bool> ExistAsync(Guid categoryId, CancellationToken cancellationToken = default);
}

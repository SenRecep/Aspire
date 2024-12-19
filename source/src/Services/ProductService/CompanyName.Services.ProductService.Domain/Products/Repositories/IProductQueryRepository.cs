using CSharpEssentials;
using CompanyName.Services.ProductService.Domain.Products.Fields;
using CompanyName.Services.ProductService.Domain.Products.ReadModels;

namespace CompanyName.Services.ProductService.Domain.Products.Repositories;
public interface IProductQueryRepository
{
    Task<Maybe<ProductReadModel>> GetProductByIdAsync(ProductId productId, CancellationToken cancellationToken = default);
    Task<ProductReadModel[]> GetProductsByCategoryId(CategoryId categoryId, CancellationToken cancellationToken = default);
}

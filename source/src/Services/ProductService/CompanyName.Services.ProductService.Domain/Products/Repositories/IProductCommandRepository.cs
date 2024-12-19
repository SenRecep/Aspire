using CSharpEssentials;
using CompanyName.Services.ProductService.Domain.Products.Fields;
using CompanyName.Services.ProductService.Domain.Products.Parameters;

namespace CompanyName.Services.ProductService.Domain.Products.Repositories;
public interface IProductCommandRepository
{
    Task<Result<ProductId>> CreateProductAsync(ProductCreateParameters parameters, CancellationToken cancellationToken = default);
    Task<Result<ProductId>> CreateProductAsync(ProductCreateParameters parameters, IRuleBase<ProductCreateParameters> rule, CancellationToken cancellationToken = default);
    Task<Result> DeleteProductAsync(ProductId productId, CancellationToken cancellationToken = default);
}

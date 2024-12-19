using CSharpEssentials;
using CompanyName.Services.ProductService.Domain.Products.Fields;
using CompanyName.Services.ProductService.Domain.Products.ReadModels;
using CompanyName.Services.ProductService.Domain.Products.Repositories;
using CompanyName.Services.ProductService.Persistence.EntityFrameworkCore.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CompanyName.Services.ProductService.Persistence.EntityFrameworkCore.Repositories.Products;
internal sealed class EfProductQueryRepository(
    ApplicationReadDbContext context) : IProductQueryRepository
{
    public async Task<Maybe<ProductReadModel>> GetProductByIdAsync(ProductId productId, CancellationToken cancellationToken = default)
    {
        return await context.Products
            .Where(product => product.Id == productId.Value)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<ProductReadModel[]> GetProductsByCategoryId(CategoryId categoryId, CancellationToken cancellationToken = default)
    {
        return context.Products
            .Where(product => product.Category == categoryId.Value)
            .OrderByDescending(product => product.CreatedAt)
            .ToArrayAsync(cancellationToken);
    }
}

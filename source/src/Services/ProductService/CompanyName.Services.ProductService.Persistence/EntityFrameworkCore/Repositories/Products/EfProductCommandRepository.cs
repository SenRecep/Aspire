using CSharpEssentials;
using CompanyName.Services.ProductService.Domain.Products;
using CompanyName.Services.ProductService.Domain.Products.Fields;
using CompanyName.Services.ProductService.Domain.Products.Parameters;
using CompanyName.Services.ProductService.Domain.Products.Repositories;
using CompanyName.Services.ProductService.Persistence.EntityFrameworkCore.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CompanyName.Services.ProductService.Persistence.EntityFrameworkCore.Repositories.Products;

internal sealed class EfProductCommandRepository(
    ApplicationWriteDbContext context) : IProductCommandRepository
{
    public async Task<Result<ProductId>> CreateProductAsync(ProductCreateParameters parameters, CancellationToken cancellationToken = default)
    {
        Result<Product> productResult = Product.Create(parameters);
        if (productResult.IsFailure)
            return productResult.Errors;
        Product product = productResult.Value;

        await context.Products.AddAsync(product, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return product.Id;
    }

    public async Task<Result<ProductId>> CreateProductAsync(ProductCreateParameters parameters, IRuleBase<ProductCreateParameters> rule, CancellationToken cancellationToken = default)
    {
        Result<Product> productResult = Product.Create(parameters, rule, cancellationToken);
        if (productResult.IsFailure)
            return productResult.Errors;
        Product product = productResult.Value;

        await context.Products.AddAsync(product, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return product.Id;
    }

    public async Task<Result> DeleteProductAsync(ProductId productId, CancellationToken cancellationToken = default)
    {
        Product? found = await context.Products
            .Where(product => product.Id == productId)
            .FirstOrDefaultAsync(cancellationToken);

        if (found is null)
            return ProductErrors.ProductDoesNotExistError(productId);

        found.Delete();
        context.Products.Remove(found);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

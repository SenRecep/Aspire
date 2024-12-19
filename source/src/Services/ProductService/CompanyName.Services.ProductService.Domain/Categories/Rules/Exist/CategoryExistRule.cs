using CSharpEssentials;
using CompanyName.Services.ProductService.Domain.Categories.Services;
using CompanyName.Services.ProductService.Domain.Products;
using CompanyName.Services.ProductService.Domain.Products.Parameters;

namespace CompanyName.Services.ProductService.Domain.Categories.Rules.Exist;
public readonly record struct CategoryExistRule
    (ICategoryService Service) : IAsyncRule<ProductCreateParameters>
{
    public async ValueTask<Result> EvaluateAsync(ProductCreateParameters context, CancellationToken cancellationToken = default)
    {
        return await Service.CategoryExistsAsync(context.Category, cancellationToken)
             .Match(
                 onSuccess: isExist => isExist.IsTrue() ?
                 Result.Success() :
                 ProductErrors.CategoryDoesNotExistError(context.Category),
                 onError: errors => errors.ToArray(),
                 cancellationToken: cancellationToken
             );

    }
}

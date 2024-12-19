using CompanyName.Services.ProductService.Domain.Products.Fields;
using CompanyName.Services.ProductService.Domain.Products.ReadModels;

namespace CompanyName.Services.ProductService.Application.Products.v1.Models;
public readonly record struct ProductViewModel
{
    private ProductViewModel(ProductReadModel product)
    {
        Id = product.Id;
        Name = product.Name;
        Description = product.Description;
        Price = product.Price;
        Currency = product.Currency;
        Category = product.Category;
        CreatedAt = product.CreatedAt;
    }

    public readonly Guid Id { get; init; }
    public readonly string Name { get; init; }
    public readonly string Description { get; init; }
    public readonly decimal Price { get; init; }
    public readonly Currency Currency { get; init; }
    public readonly Guid Category { get; init; }
    public readonly DateTimeOffset CreatedAt { get; init; }

    public static ProductViewModel Create(ProductReadModel product) => new(product);
    public static ProductViewModel[] Create(ProductReadModel[] products) => [.. products.Select(Create)];
}

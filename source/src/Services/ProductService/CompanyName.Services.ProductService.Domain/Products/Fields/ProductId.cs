using CSharpEssentials;

namespace CompanyName.Services.ProductService.Domain.Products.Fields;

public readonly record struct ProductId
{
    private ProductId(Guid value) => Value = value;
    public Guid Value { get; }
    public static ProductId New() => new(Guider.NewGuid());
    public static ProductId From(Guid value) => new(value);

    public static readonly ProductId Empty = new(Guid.Empty);
}

using CSharpEssentials;

namespace CompanyName.Services.ProductService.Domain.Products.Fields;

public readonly record struct ProductName
{
    public const int MinLength = 2;
    public const int MaxLength = 100;
    public string Value { get; }
    private ProductName(string value) => Value = value;
    public static ProductName From(string value) => new(value);
    public static Result<ProductName> Create(string? value)
    {
        if (value.IsEmpty())
            return ProductErrors.Name.EmptyError;

        if (value.Length < MinLength)
            return ProductErrors.Name.TooShortError(value.Length);

        if (value.Length > MaxLength)
            return ProductErrors.Name.TooLongError(value.Length);

        return new ProductName(value);
    }
}

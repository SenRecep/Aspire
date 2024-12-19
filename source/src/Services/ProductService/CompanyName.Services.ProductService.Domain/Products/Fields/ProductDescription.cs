using CSharpEssentials;

namespace CompanyName.Services.ProductService.Domain.Products.Fields;

public readonly record struct ProductDescription
{
    public const int MinLength = 10;
    public const int MaxLength = 1_000;
    public string Value { get; }
    private ProductDescription(string value) => Value = value;
    public static ProductDescription From(string value) => new(value);
    public static Result<ProductDescription> Create(string? value)
    {
        if (value.IsEmpty())
            return ProductErrors.Description.EmptyError;
        if (value.Length < MinLength)
            return ProductErrors.Description.TooShortError(value.Length);
        if (value.Length > MaxLength)
            return ProductErrors.Description.TooLongError(value.Length);
        return new ProductDescription(value);
    }
}

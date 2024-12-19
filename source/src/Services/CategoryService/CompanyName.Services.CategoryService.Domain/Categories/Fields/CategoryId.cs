using CSharpEssentials;

namespace CompanyName.Services.CategoryService.Domain.Categories.Fields;

public readonly record struct CategoryId
{
    private CategoryId(Guid value) => Value = value;
    public Guid Value { get; }
    public static CategoryId New() => new(Guider.NewGuid());
    public static CategoryId From(Guid value) => new(value);

    public static readonly CategoryId Empty = new(Guid.Empty);
}

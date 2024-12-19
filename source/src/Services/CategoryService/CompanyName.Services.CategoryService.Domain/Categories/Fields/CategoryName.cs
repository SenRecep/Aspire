using CSharpEssentials;

namespace CompanyName.Services.CategoryService.Domain.Categories.Fields;

public readonly record struct CategoryName
{
    public const int MinLength = 2;
    public const int MaxLength = 100;
    public string Value { get; }
    private CategoryName(string value) => Value = value;
    public static CategoryName From(string value) => new(value);
    public static Result<CategoryName> Create(string? value)
    {
        if (value.IsEmpty())
            return CategoryErrors.Name.EmptyError;

        if (value.Length < MinLength)
            return CategoryErrors.Name.TooShortError(value.Length);

        if (value.Length > MaxLength)
            return CategoryErrors.Name.TooLongError(value.Length);

        return new CategoryName(value);
    }
}

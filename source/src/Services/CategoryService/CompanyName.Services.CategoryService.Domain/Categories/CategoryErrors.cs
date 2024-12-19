using CSharpEssentials;
using CompanyName.Services.CategoryService.Domain.Categories.Fields;

namespace CompanyName.Services.CategoryService.Domain.Categories;
public static class CategoryErrors
{
    public static Error CategoryDoesNotExistError(CategoryId category) => Error.NotFound(code: "Category.DoesNotExist", description: $"Category does not exist: {category.Value}");
    public static class Name
    {
        public static readonly Error EmptyError =
            Error.Validation(code: "Category.Name.Empty", description: "Category name is required");
        public static Error TooShortError(int length) =>
            Error.Validation(code: "Category.Name.TooShort", description: $"Category name is too short length: {length}");
        public static Error TooLongError(int length) =>
            Error.Validation(code: "Category.Name.TooLong", description: $"Category name is too long length: {length}");
    }
}

using CSharpEssentials;
using CompanyName.Services.ProductService.Domain.Products.Fields;

namespace CompanyName.Services.ProductService.Domain.Products;
public static class ProductErrors
{
    public static readonly Error EmptyCategoryIdError = Error.Validation(code: "Product.Category.Empty", description: "Product category is required");
    public static Error CategoryDoesNotExistError(CategoryId category) => Error.NotFound(code: "Product.Category.DoesNotExist", description: $"Product category does not exist: {category.Value}");
    public static Error ProductDoesNotExistError(ProductId product) => Error.NotFound(code: "Product.DoesNotExist", description: $"Product does not exist: {product.Value}");
    public static class Name
    {
        public static readonly Error EmptyError =
            Error.Validation(code: "Product.Name.Empty", description: "Product name is required");
        public static Error TooShortError(int length) =>
            Error.Validation(code: "Product.Name.TooShort", description: $"Product name is too short length: {length}");
        public static Error TooLongError(int length) =>
            Error.Validation(code: "Product.Name.TooLong", description: $"Product name is too long length: {length}");
    }

    public static class Description
    {
        public static readonly Error EmptyError =
            Error.Validation(code: "Product.Description.Empty", description: "Product description is required");
        public static Error TooShortError(int length) =>
            Error.Validation(code: "Product.Description.TooShort", description: $"Product description is too short length: {length}");
        public static Error TooLongError(int length) =>
            Error.Validation(code: "Product.Description.TooLong", description: $"Product description is too long length: {length}");
    }
}

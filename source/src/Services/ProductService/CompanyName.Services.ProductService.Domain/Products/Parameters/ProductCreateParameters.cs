using CompanyName.Services.ProductService.Domain.Products.Fields;

namespace CompanyName.Services.ProductService.Domain.Products.Parameters;
public sealed record ProductCreateParameters(string? Name, string? Description, decimal Price, Currency Currency, CategoryId Category);

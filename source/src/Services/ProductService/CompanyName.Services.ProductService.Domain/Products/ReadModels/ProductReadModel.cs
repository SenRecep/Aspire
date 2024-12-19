using CSharpEssentials.Interfaces;
using CompanyName.Services.ProductService.Domain.Products.Fields;

namespace CompanyName.Services.ProductService.Domain.Products.ReadModels;
public sealed class ProductReadModel : ISoftDeletableBase
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public Currency Currency { get; set; }
    public Guid Category { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
}

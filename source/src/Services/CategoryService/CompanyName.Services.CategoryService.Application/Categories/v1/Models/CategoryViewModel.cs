using CompanyName.Services.CategoryService.Domain.Categories.ReadModels;

namespace CompanyName.Services.CategoryService.Application.Products.v1.Models;
public readonly record struct CategoryViewModel
{
    private CategoryViewModel(CategoryReadModel categoy)
    {
        Id = categoy.Id;
        Name = categoy.Name;
        CreatedAt = categoy.CreatedAt;
    }

    public readonly Guid Id { get; init; }
    public readonly string Name { get; init; }
    public readonly DateTimeOffset CreatedAt { get; init; }

    public static CategoryViewModel Create(CategoryReadModel categoy) => new(categoy);
    public static CategoryViewModel[] Create(CategoryReadModel[] categories) => [.. categories.Select(Create)];
}

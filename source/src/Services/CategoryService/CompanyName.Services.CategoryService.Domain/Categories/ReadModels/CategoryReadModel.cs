namespace CompanyName.Services.CategoryService.Domain.Categories.ReadModels;
public sealed class CategoryReadModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

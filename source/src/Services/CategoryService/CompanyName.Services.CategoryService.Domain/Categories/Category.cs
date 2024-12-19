using CSharpEssentials;
using CSharpEssentials.Entity;
using CompanyName.Services.CategoryService.Domain.Categories.Events;
using CompanyName.Services.CategoryService.Domain.Categories.Fields;
using CompanyName.Services.CategoryService.Domain.Categories.Parameters;

namespace CompanyName.Services.CategoryService.Domain.Categories;
public sealed class Category : EntityBase<CategoryId>
{
    private Category() { }
    private Category(CategoryId productId, CategoryName name)
    {
        Id = productId;
        Name = name;
    }
    public CategoryName Name { get; private set; }

    public static Result<Category> Create(
        CategoryCreateParameters parameters)
    {
        Result<CategoryName> name = CategoryName.Create(parameters.Name);

        if (name.IsFailure)
            return name.Errors;


        return new Category(CategoryId.New(), name.Value);
    }

    public void Delete() => Raise(new CategoryDeletedDomainEvent(Id));
}

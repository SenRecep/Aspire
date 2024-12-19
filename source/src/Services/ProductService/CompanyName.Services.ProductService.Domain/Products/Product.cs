using CSharpEssentials;
using CSharpEssentials.Entity;
using CompanyName.Services.ProductService.Domain.Products.Events;
using CompanyName.Services.ProductService.Domain.Products.Fields;
using CompanyName.Services.ProductService.Domain.Products.Parameters;

namespace CompanyName.Services.ProductService.Domain.Products;
public sealed class Product : SoftDeletableEntityBase<ProductId>
{
    private Product() { }
    private Product(ProductId productId, ProductName name, ProductDescription description, CategoryId category, Money money)
    {
        Id = productId;
        Name = name;
        Description = description;
        Category = category;
        Money = money;
        Raise(new ProductCreatedDomainEvent(productId));
    }
    public ProductName Name { get; private set; }
    public ProductDescription Description { get; private set; }
    public Money Money { get; private set; }
    public CategoryId Category { get; private set; }

    public static Result<Product> Create(
        ProductCreateParameters parameters)
    {
        Result<ProductName> productName = ProductName.Create(parameters.Name);
        Result<ProductDescription> productDescription = ProductDescription.Create(parameters.Description);

        var result = Result.And(productName, productDescription);
        if (result.IsFailure)
            return result.Errors;

        if (parameters.Category == CategoryId.Empty)
            return ProductErrors.EmptyCategoryIdError;

        var money = Money.From(parameters.Price, parameters.Currency);

        return new Product(ProductId.New(), productName.Value, productDescription.Value, parameters.Category, money);
    }

    public static Result<Product> Create(
        ProductCreateParameters parameters,
        IRuleBase<ProductCreateParameters> rule,
        CancellationToken cancellationToken = default)
    {
        Result ruleResult = RuleEngine.Evaluate(rule, parameters, cancellationToken);
        return ruleResult.Match(
            () => Create(parameters),
            errors => errors);
    }

    public void Delete() => Raise(new ProductDeletedDomainEvent(Id));
}

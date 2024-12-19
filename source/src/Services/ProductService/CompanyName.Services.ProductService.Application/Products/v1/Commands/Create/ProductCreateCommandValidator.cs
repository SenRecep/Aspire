using FluentValidation;
using CompanyName.Services.ProductService.Domain.Products.Fields;

namespace CompanyName.Services.ProductService.Application.Products.v1.Commands.Create;

internal sealed class ProductCreateCommandValidator : AbstractValidator<ProductCreateCommand>
{
    public ProductCreateCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(ProductName.MinLength)
            .MaximumLength(ProductName.MaxLength);
        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(ProductDescription.MinLength)
            .MaximumLength(ProductDescription.MaxLength);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.Currency).IsInEnum();
        RuleFor(x => x.Category).NotEmpty();
    }
}

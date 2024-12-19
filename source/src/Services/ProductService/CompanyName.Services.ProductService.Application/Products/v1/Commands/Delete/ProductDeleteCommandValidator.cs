using FluentValidation;

namespace CompanyName.Services.ProductService.Application.Products.v1.Commands.Delete;

internal sealed class ProductDeleteCommandValidator : AbstractValidator<ProductDeleteCommand>
{
    public ProductDeleteCommandValidator() => RuleFor(x => x.ProductId).NotEmpty();
}


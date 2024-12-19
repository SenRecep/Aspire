using FluentValidation;

namespace CompanyName.Services.ProductService.Application.Products.v1.Queries.Get;

internal sealed class GetProductQueryValidator : AbstractValidator<GetProductQuery>
{
    public GetProductQueryValidator() => RuleFor(x => x.ProductId).NotEmpty();
}

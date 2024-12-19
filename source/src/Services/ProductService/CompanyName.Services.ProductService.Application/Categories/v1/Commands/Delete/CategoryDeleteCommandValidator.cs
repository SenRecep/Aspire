using FluentValidation;

namespace CompanyName.Services.ProductService.Application.Categories.v1.Commands.Delete;

internal sealed class CategoryDeleteCommandValidator : AbstractValidator<CategoryDeleteCommand>
{
    public CategoryDeleteCommandValidator() => RuleFor(x => x.CategoryId).NotEmpty();
}

using FluentValidation;
using CompanyName.Services.CategoryService.Domain.Categories.Fields;

namespace CompanyName.Services.CategoryService.Application.Categories.v1.Commands.Create;

internal sealed class CategoryCreateCommandValidator : AbstractValidator<CategoryCreateCommand>
{
    public CategoryCreateCommandValidator() =>
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(CategoryName.MinLength)
            .MaximumLength(CategoryName.MaxLength);
}

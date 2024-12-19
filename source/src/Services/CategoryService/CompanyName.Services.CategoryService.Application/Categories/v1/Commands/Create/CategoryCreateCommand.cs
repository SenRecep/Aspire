using CompanyName.BuildingBlocks.Application.Abstractions.Contracts;
using CompanyName.Services.CategoryService.Domain.Categories.Fields;
using CompanyName.Services.CategoryService.Domain.Categories.Parameters;

namespace CompanyName.Services.CategoryService.Application.Categories.v1.Commands.Create;

public sealed record CategoryCreateCommand(
    string? Name) : ICommand<CategoryId>
{
    public CategoryCreateParameters ToParameters() =>
        new(Name);
}

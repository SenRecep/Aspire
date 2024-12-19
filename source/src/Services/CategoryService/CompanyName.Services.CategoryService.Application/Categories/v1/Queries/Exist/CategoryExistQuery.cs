using CompanyName.BuildingBlocks.Application.Abstractions.Contracts;

namespace CompanyName.Services.CategoryService.Application.Categories.v1.Queries.Exist;
public sealed record CategoryExistQuery(Guid CategoryId) : IQuery<bool>;

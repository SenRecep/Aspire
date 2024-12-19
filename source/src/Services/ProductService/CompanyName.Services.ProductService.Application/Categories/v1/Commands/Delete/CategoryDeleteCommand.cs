using CompanyName.BuildingBlocks.Application.Abstractions.Contracts;

namespace CompanyName.Services.ProductService.Application.Categories.v1.Commands.Delete;
public sealed record CategoryDeleteCommand(Guid CategoryId) : ICommand;

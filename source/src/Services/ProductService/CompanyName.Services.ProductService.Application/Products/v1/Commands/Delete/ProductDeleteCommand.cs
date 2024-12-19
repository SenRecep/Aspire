using CompanyName.BuildingBlocks.Application.Abstractions.Contracts;

namespace CompanyName.Services.ProductService.Application.Products.v1.Commands.Delete;
public sealed record ProductDeleteCommand(Guid ProductId) : ICommand;

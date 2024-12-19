using CSharpEssentials;
using MediatR;

namespace CompanyName.BuildingBlocks.Application.Abstractions.Contracts;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;

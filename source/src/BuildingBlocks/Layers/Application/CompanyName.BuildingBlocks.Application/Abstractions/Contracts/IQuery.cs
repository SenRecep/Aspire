using CSharpEssentials;
using MediatR;

namespace CompanyName.BuildingBlocks.Application.Abstractions.Contracts;

public interface IQuery : IRequest<Result>;
public interface IQuery<TResponse> : IRequest<Result<TResponse>>;

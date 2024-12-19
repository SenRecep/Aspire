using CSharpEssentials;
using MediatR;

namespace CompanyName.BuildingBlocks.Application.Abstractions.Contracts;

public interface IQueryHandler<TQuery> : IRequestHandler<TQuery, Result> where TQuery : IQuery;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse>;

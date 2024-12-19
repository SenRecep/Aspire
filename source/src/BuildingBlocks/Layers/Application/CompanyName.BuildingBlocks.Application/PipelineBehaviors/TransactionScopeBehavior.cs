using MediatR;
using CompanyName.BuildingBlocks.Application.Abstractions.Contracts;
using System.Transactions;

namespace CompanyName.BuildingBlocks.Application.PipelineBehaviors;
public sealed class TransactionScopeBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ITransactionalRequest
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        using TransactionScope transactionScope = new(TransactionScopeAsyncFlowOption.Enabled);
        TResponse response = await next();
        transactionScope.Complete();
        return response;
    }
}

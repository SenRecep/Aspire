using CSharpEssentials;
using CSharpEssentials.Interfaces;
using MediatR;
using CompanyName.BuildingBlocks.Application.Abstractions.Contracts;
using CompanyName.BuildingBlocks.Caching.Base;
using Microsoft.Extensions.Logging;

namespace CompanyName.BuildingBlocks.Application.PipelineBehaviors;
public sealed class CachingBehavior<TRequest, TResponse>
    (ILogger<CachingBehavior<TRequest, TResponse>> logger,
    ICacheService cacheService) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheable
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request.BypassCache)
            return await next();
        if (cacheService.TryGet(request.CacheKey, out Maybe<TResponse> response))
        {
            logger.LogInformation("Cache hit for key {CacheKey}", request.CacheKey);
            return response.Value;
        }

        logger.LogInformation("Cache miss for key {CacheKey}", request.CacheKey);
        TResponse result = await next();
        if (request.CacheFailures.IsFalse() && result is IResultBase r && r.IsFailure)
            return result;
        cacheService.Set(request.CacheKey, result, request.Expiration, request.Tags);
        logger.LogInformation("Cache set for key {CacheKey}", request.CacheKey);
        return result;
    }
}

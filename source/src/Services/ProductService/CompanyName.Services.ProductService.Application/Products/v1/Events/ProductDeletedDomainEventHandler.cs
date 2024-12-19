using MediatR;
using CompanyName.BuildingBlocks.Caching.Base;
using CompanyName.Services.ProductService.Domain.Products.Events;
using Microsoft.Extensions.Logging;

namespace CompanyName.Services.ProductService.Application.Products.v1.Events;

internal sealed class ProductDeletedDomainEventHandler(
    ILogger<ProductCreatedDomainEventHandler> logger,
    ICacheService cacheService) : INotificationHandler<ProductDeletedDomainEvent>
{
    private const string Tag = "products:list";
    public Task Handle(ProductDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("ProductDeletedDomainEvent handled");
        string productIdCache = $"product:{notification.Id}";
        cacheService.Remove(productIdCache);
        return cacheService.InvalidateTagAsync(Tag);
    }
}

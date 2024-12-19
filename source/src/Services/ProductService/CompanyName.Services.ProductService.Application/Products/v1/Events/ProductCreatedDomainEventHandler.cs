using MediatR;
using CompanyName.BuildingBlocks.Caching.Base;
using CompanyName.Services.ProductService.Domain.Products.Events;
using Microsoft.Extensions.Logging;

namespace CompanyName.Services.ProductService.Application.Products.v1.Events;
internal sealed class ProductCreatedDomainEventHandler(
    ILogger<ProductCreatedDomainEventHandler> logger,
    ICacheService cacheService) : INotificationHandler<ProductCreatedDomainEvent>
{
    private const string Tag = "products";

    public async Task Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("ProductCreatedDomainEvent handled");
        await cacheService.InvalidateTagAsync(Tag);
    }
}

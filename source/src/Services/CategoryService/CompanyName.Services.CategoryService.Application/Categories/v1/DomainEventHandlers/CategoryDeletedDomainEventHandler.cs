using MediatR;
using CompanyName.BuildingBlocks.Caching.Base;
using CompanyName.BuildingBlocks.MessageBrokers.Base;
using CompanyName.IntegrationEvents.Categories;
using CompanyName.Services.CategoryService.Domain.Categories.Events;
using Microsoft.Extensions.Logging;

namespace CompanyName.Services.CategoryService.Application.Categories.v1.DomainEventHandlers;

internal sealed class CategoryDeletedDomainEventHandler(
    ILogger<CategoryDeletedDomainEvent> logger,
    IEventBus eventBus,
    ICacheService cacheService) : INotificationHandler<CategoryDeletedDomainEvent>
{
    public async Task Handle(CategoryDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("CategoryDeletedDomainEvent handled");
        string CategoryIdCache = $"category:{notification.Id}";
        cacheService.Remove(CategoryIdCache);
        await eventBus.PublishAsync(new CategoryDeletedIntegrationEvent(notification.Id.Value), isTransactional: false, cancellationToken);
    }
}

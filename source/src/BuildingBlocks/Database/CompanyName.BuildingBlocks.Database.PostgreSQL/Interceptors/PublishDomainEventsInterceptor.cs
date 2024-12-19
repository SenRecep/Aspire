using CSharpEssentials;
using CSharpEssentials.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CompanyName.BuildingBlocks.Database.PostgreSQL.Interceptors;

internal sealed class PublishDomainEventsInterceptor(
    ILogger<PublishDomainEventsInterceptor> logger,
    IServiceScopeFactory serviceScopeFactory) : SaveChangesInterceptor
{

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        IDomainEvent[] domainEvents = eventData.Context.IsNotNull() ? GetEvents(eventData.Context) : [];
        InterceptionResult<int> returnValue = await base.SavingChangesAsync(eventData, result, cancellationToken);
        await PublishDomainEventsAsync(domainEvents, cancellationToken);
        return returnValue;
    }

    private IDomainEvent[] GetEvents(DbContext context)
    {
        return [.. context
            .ChangeTracker
            .Entries<IDomainEventHolder>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                IReadOnlyList<IDomainEvent> domainEvents = entity.DomainEvents;
                entity.ClearDomainEvents();
                return domainEvents;
            })];
    }

    private async Task PublishDomainEventsAsync(IDomainEvent[] domainEvents, CancellationToken cancellationToken)
    {
        IServiceProvider serviceProvider = serviceScopeFactory.CreateScope().ServiceProvider;
        IPublisher publisher = serviceProvider.GetRequiredService<IPublisher>();
        logger.LogInformation("Publishing {Count} domain events", domainEvents.Length);

        foreach (IDomainEvent domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent, cancellationToken);
        }

        logger.LogInformation("Published {Count} domain events", domainEvents.Length);
    }
}

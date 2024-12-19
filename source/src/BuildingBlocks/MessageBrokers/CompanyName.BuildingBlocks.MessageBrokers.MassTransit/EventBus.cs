using MassTransit;
using CompanyName.BuildingBlocks.MessageBrokers.Base;

namespace CompanyName.BuildingBlocks.MessageBrokers.MassTransit;
internal sealed class EventBus(
    IPublishEndpoint publishEndpoint,
    IBus bus,
    bool isOutboxEnabled) : IEventBus
{
    private Task PublishToOutbox<TMessage>(TMessage message, CancellationToken cancellationToken)
        where TMessage : class =>
        publishEndpoint.Publish(message, cancellationToken);

    private Task PublishDirectly<TMessage>(TMessage message, CancellationToken cancellationToken)
        where TMessage : class =>
        bus.Publish(message, cancellationToken);

    public async Task PublishAsync<TMessage>(TMessage message, bool isTransactional = true, CancellationToken cancellationToken = default) where TMessage : class
    {
        if (!isOutboxEnabled || isTransactional)
            await PublishToOutbox(message, cancellationToken);
        else
            await PublishDirectly(message, cancellationToken);
    }
}

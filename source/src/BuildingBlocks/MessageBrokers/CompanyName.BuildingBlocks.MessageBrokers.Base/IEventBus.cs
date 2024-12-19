namespace CompanyName.BuildingBlocks.MessageBrokers.Base;
public interface IEventBus
{
    Task PublishAsync<TMessage>(TMessage message, bool isTransactional = true, CancellationToken cancellationToken = default)
        where TMessage : class;
}

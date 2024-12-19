using MassTransit;
using Microsoft.Extensions.Logging;

namespace CompanyName.BuildingBlocks.MessageBrokers.MassTransit.Observers;

public sealed class ConsumeLoggerObserver(
    ILogger<ConsumeLoggerObserver> logger) : IConsumeObserver
{
    public Task ConsumeFault<T>(ConsumeContext<T> context, Exception exception) where T : class
    {
        logger.LogError(exception, "Error consuming message: {@Message}", context.Message);
        throw new NotImplementedException();
    }

    public Task PostConsume<T>(ConsumeContext<T> context) where T : class
    {
        logger.LogInformation("Consumed message: {@Message}", context.Message);
        return Task.CompletedTask;
    }

    public Task PreConsume<T>(ConsumeContext<T> context) where T : class
    {
        logger.LogInformation("Consuming message: {@Message}", context.Message);
        return Task.CompletedTask;
    }
}

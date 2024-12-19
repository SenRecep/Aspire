using MassTransit;
using Microsoft.Extensions.Logging;

namespace CompanyName.BuildingBlocks.MessageBrokers.MassTransit.Observers;
public sealed class PublishLoggerObserver(
    ILogger<PublishLoggerObserver> logger) : IPublishObserver
{
    public Task PostPublish<T>(PublishContext<T> context) where T : class
    {
        logger.LogInformation("Published message: {@Message}", context.Message);
        return Task.CompletedTask;
    }

    public Task PrePublish<T>(PublishContext<T> context) where T : class
    {
        logger.LogInformation("Publishing message: {@Message}", context.Message);
        return Task.CompletedTask;
    }

    public Task PublishFault<T>(PublishContext<T> context, Exception exception) where T : class
    {
        logger.LogError(exception, "Error publishing message: {@Message}", context.Message);
        return Task.CompletedTask;
    }
}

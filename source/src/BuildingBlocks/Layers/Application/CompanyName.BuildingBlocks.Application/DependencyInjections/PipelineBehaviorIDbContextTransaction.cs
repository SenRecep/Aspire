using CompanyName.BuildingBlocks.Application.PipelineBehaviors;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.BuildingBlocks.Application.DependencyInjections;
public static class PipelineBehaviorIDbContextTransaction
{
    public static MediatRServiceConfiguration AddCachingBehavior(this MediatRServiceConfiguration configuration) =>
           configuration.AddOpenBehavior(typeof(CachingBehavior<,>));
    public static MediatRServiceConfiguration AddTransactionBehavior(this MediatRServiceConfiguration configuration) =>
           configuration.AddOpenBehavior(typeof(TransactionScopeBehavior<,>));
    public static MediatRServiceConfiguration AddLoggingBehavior(this MediatRServiceConfiguration configuration) =>
           configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
    public static MediatRServiceConfiguration AddValidationBehavior(this MediatRServiceConfiguration configuration) =>
           configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
    public static MediatRServiceConfiguration AddDefaultBehaviors(this MediatRServiceConfiguration configuration) =>
        configuration
            .AddLoggingBehavior()
            .AddValidationBehavior()
            .AddCachingBehavior()
            .AddTransactionBehavior();

    public static MediatRServiceConfiguration AddBehaviors(this MediatRServiceConfiguration configuration, IEnumerable<PipelineBehaviorType> pipelines)
    {
        foreach (PipelineBehaviorType pipeline in pipelines)
        {
            switch (pipeline)
            {
                case PipelineBehaviorType.Logging:
                    configuration.AddLoggingBehavior();
                    break;
                case PipelineBehaviorType.Validation:
                    configuration.AddValidationBehavior();
                    break;
                case PipelineBehaviorType.Caching:
                    configuration.AddCachingBehavior();
                    break;
                case PipelineBehaviorType.Transaction:
                    configuration.AddTransactionBehavior();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        return configuration;
    }
}

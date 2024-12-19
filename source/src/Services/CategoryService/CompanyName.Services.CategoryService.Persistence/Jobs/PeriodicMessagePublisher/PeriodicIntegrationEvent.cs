namespace CompanyName.IntegrationEvents.Jobs;

public sealed record PeriodicIntegrationEvent(string JobInstanceId, DateTime Timestamp);
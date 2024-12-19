namespace CompanyName.BuildingBlocks.Application.Abstractions.Contracts;

public interface ICacheable
{
    bool BypassCache { get; }
    bool CacheFailures { get; }
    string CacheKey { get; }
    TimeSpan Expiration { get; }
    string[] Tags { get; }
}

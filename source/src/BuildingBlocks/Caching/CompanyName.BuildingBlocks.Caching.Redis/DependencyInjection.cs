using CompanyName.BuildingBlocks.Caching.Base;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Trace;

namespace CompanyName.BuildingBlocks.Caching.Redis;
public static class DependencyInjection
{
    public static IServiceCollection AddCacheServices(
        this IServiceCollection services,
        string serviceName,
        Action<RedisCacheOptions> redisSetupAction,
        Action<MemoryCacheOptions> memorySetupAction)
    {
        services.AddStackExchangeRedisCache(redisSetupAction);
        services.AddMemoryCache(memorySetupAction);
        services.AddSingleton<CacheOptions>(new CacheOptions(serviceName));
        services.AddKeyedSingleton<ICacheService, RedisCacheService>(CacheServiceType.Redis);
        services.AddKeyedSingleton<ICacheService, MemoryCacheService>(CacheServiceType.Memory);
        services.AddSingleton<ICacheService, CacheService>();
        return services;
    }

    public static IServiceCollection ConfigureCacheServiceTelemetry(
        this IServiceCollection services) =>
        services
        .ConfigureOpenTelemetryTracerProvider(configure => configure.AddRedisInstrumentation());
}

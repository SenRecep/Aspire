using CSharpEssentials;
using CompanyName.BuildingBlocks.Caching.Base;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.BuildingBlocks.Caching.Redis;
internal sealed class CacheService(
    [FromKeyedServices(CacheServiceType.Redis)] ICacheService redisCacheService,
    [FromKeyedServices(CacheServiceType.Memory)] ICacheService memoryCacheService) : ICacheService
{
    public bool IsAvailable => redisCacheService.IsAvailable || memoryCacheService.IsAvailable;

    private ICacheService GetAvailableService()
    {
        if (redisCacheService.IsAvailable)
            return redisCacheService;

        return memoryCacheService;
    }

    public Maybe<T> Get<T>(string key) =>
        GetAvailableService().Get<T>(key);

    public Task<string[]> GetKeysByPatternAsync(string pattern) =>
        GetAvailableService().GetKeysByPatternAsync(pattern);
    public T GetOrCreate<T>(string key, Func<T> valueFactory, TimeSpan? expiration, params string[] tags) =>
        GetAvailableService().GetOrCreate(key, valueFactory, expiration, tags);

    public Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> valueFactory, TimeSpan? expiration, params string[] tags) =>
        GetAvailableService().GetOrCreateAsync(key, valueFactory, expiration, tags);

    public void InvalidateTag(string tag) =>
        GetAvailableService().InvalidateTag(tag);

    public Task InvalidateTagAsync(string tag) =>
        GetAvailableService().InvalidateTagAsync(tag);

    public void InvalidateTags(params string[] tags) =>
        GetAvailableService().InvalidateTags(tags);

    public Task InvalidateTagsAsync(params string[] tags) =>
        GetAvailableService().InvalidateTagsAsync(tags);

    public bool Lock(string key, TimeSpan expiration) =>
        GetAvailableService().Lock(key, expiration);

    public Task<bool> LockAsync(string key, TimeSpan expiration) =>
        GetAvailableService().LockAsync(key, expiration);

    public void Remove(string key) =>
        GetAvailableService().Remove(key);

    public Task RemoveByPatternAsync(string pattern) =>
        GetAvailableService().RemoveByPatternAsync(pattern);

    public void RemoveKeys(string[] keys) =>
        GetAvailableService().RemoveKeys(keys);

    public T Set<T>(string key, T value, TimeSpan? expiration, params string[] tags) =>
        GetAvailableService().Set(key, value, expiration, tags);

    public bool TryGet<T>(string key, out Maybe<T> value) =>
        GetAvailableService().TryGet(key, out value);

    public bool Unlock(string key) =>
        GetAvailableService().Unlock(key);

    public Task<bool> UnlockAsync(string key) =>
        GetAvailableService().UnlockAsync(key);
}

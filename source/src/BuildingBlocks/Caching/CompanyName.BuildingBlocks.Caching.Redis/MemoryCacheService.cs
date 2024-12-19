using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using CSharpEssentials;
using CompanyName.BuildingBlocks.Caching.Base;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace CompanyName.BuildingBlocks.Caching.Redis;
internal sealed class MemoryCacheService(
    ILogger<MemoryCacheService> logger,
    IMemoryCache memoryCache) : ICacheService
{
    private static readonly TimeSpan _defaultExpiration = TimeSpan.FromMinutes(5);
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly ConcurrentDictionary<string, HashSet<string>> _tagsMap = [];
    private readonly ConcurrentDictionary<TimeSpan, MemoryCacheEntryOptions> _options = [];

    public bool IsAvailable => true;

    private string ConvertPatternToRegex(string pattern)
    {
        return "^" + Regex.Escape(pattern)
            .Replace("\\*", ".*")
            .Replace("\\?", ".")
            + "$";
    }
    public Maybe<T> Get<T>(string key) =>
           _memoryCache.Get<T>(key);

    public Task<string[]> GetKeysByPatternAsync(string pattern)
    {
        string regexPattern = ConvertPatternToRegex(pattern);
        var regex = new Regex(regexPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        string[] matchingKeys = [.. _tagsMap.Values
            .SelectMany(keys => keys)
            .Where(key => regex.IsMatch(key))
            .SelectMany(key => _tagsMap[key])
            .Distinct()];

        return matchingKeys.AsTask();
    }

    public T GetOrCreate<T>(string key, Func<T> valueFactory, TimeSpan? expiration, params string[] tags)
    {
        return Get<T>(key).Match(
            some: value => value,
            none: () => Set(key, valueFactory(), expiration, tags));
    }

    public Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> valueFactory, TimeSpan? expiration, params string[] tags)
    {
        return Get<T>(key).Match(
            some: value => value.AsTask(),
            none: async () => Set(key, await valueFactory(), expiration, tags));
    }

    public void InvalidateTag(string tag)
    {
        if (_tagsMap.TryRemove(tag, out HashSet<string>? keys))
        {
            foreach (string key in keys)
                Remove(key);
        }
    }

    public Task InvalidateTagAsync(string tag)
    {
        InvalidateTag(tag);
        return Task.CompletedTask;
    }

    public void InvalidateTags(params string[] tags)
    {
        foreach (string tag in tags)
            InvalidateTag(tag);
    }

    public Task InvalidateTagsAsync(params string[] tags)
    {
        InvalidateTags(tags);
        return Task.CompletedTask;
    }

    private string CreateLockKey(string key) => $"lock:{key}";
    public bool Lock(string key, TimeSpan expiration)
    {
        string lockKey = CreateLockKey(key);
        if (TryGet(lockKey, out Maybe<bool> _))
        {
            logger.LogInformation("Lock is already acquired for key {Key}", key);
            return false;
        }

        Set(lockKey, true, expiration);
        logger.LogInformation("Lock is acquired for key {Key}", key);
        return true;
    }

    public Task<bool> LockAsync(string key, TimeSpan expiration)
    {
        return Lock(key, expiration).AsTask();
    }

    public void Remove(string key)
    {
        _memoryCache.Remove(key);

        foreach (HashSet<string> keys in _tagsMap.Values)
            keys.Remove(key);
    }

    public async Task RemoveByPatternAsync(string pattern)
    {
        string[] keys = await GetKeysByPatternAsync(pattern);
        RemoveKeys(keys);
    }

    public void RemoveKeys(string[] keys)
    {
        foreach (string key in keys)
            Remove(key);
    }

    public T Set<T>(string key, T value, TimeSpan? expiration, params string[] tags)
    {
        _memoryCache.Set(key, value, GetMemoryCacheEntryOptions(expiration));
        foreach (string tag in tags)
            _tagsMap.AddOrUpdate(tag, [key], (_, keys) =>
            {
                keys.Add(key);
                return keys;
            });
        return value;
    }

    public bool TryGet<T>(string key, out Maybe<T> value)
    {
        bool isExist = _memoryCache.TryGetValue(key, out T? result);
        value = isExist ? Maybe.From(result) : Maybe.None;
        return isExist;
    }

    public bool Unlock(string key)
    {
        string lockKey = CreateLockKey(key);
        if (TryGet(lockKey, out Maybe<bool> _))
        {
            Remove(lockKey);
            logger.LogInformation("Lock is released for key {Key}", key);
            return true;
        }

        logger.LogInformation("Lock is not acquired for key {Key}", key);
        return false;
    }

    public Task<bool> UnlockAsync(string key)
    {
        return Unlock(key).AsTask();
    }

    private MemoryCacheEntryOptions GetMemoryCacheEntryOptions(TimeSpan? expiration)
    {
        return _options.GetOrAdd(expiration ?? _defaultExpiration, (ex) => new()
        {
            AbsoluteExpirationRelativeToNow = ex,
            SlidingExpiration = ex / 2
        });
    }
}

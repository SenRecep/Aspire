using System.Collections.Concurrent;
using CSharpEssentials;
using CSharpEssentials.Json;
using CompanyName.BuildingBlocks.Caching.Base;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace CompanyName.BuildingBlocks.Caching.Redis;
internal sealed class RedisCacheService(
    ILogger<RedisCacheService> logger,
    IConnectionMultiplexer connectionMultiplexer,
    CacheOptions cacheOptions) : ICacheService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer = connectionMultiplexer;
    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();
    private readonly string _serviceName = cacheOptions.ServiceName ?? nameof(CompanyName);

    public bool IsAvailable => _connectionMultiplexer.IsConnected;

    public Maybe<T> Get<T>(string key)
    {
        RedisValue data = _database.StringGet(CreateKey(key));
        if (data.IsNullOrEmpty)
            return Maybe.None;
        T value = data.ToString().ConvertFromJson<T>();
        return value;
    }

    public async Task<string[]> GetKeysByPatternAsync(string pattern)
    {
        pattern = CreateKey(pattern);
        IServer[] servers = _connectionMultiplexer.GetServers();
        var allKeys = new ConcurrentDictionary<string, bool>();

        IEnumerable<Task> tasks = servers.Select(async server =>
        {
            await foreach (RedisKey key in server.KeysAsync(pattern: pattern))
                allKeys.TryAdd(key.ToString(), true);
        });

        await Task.WhenAll(tasks);

        string[] keys = [.. allKeys.Keys];
        return keys;
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
        string tagKey = CreateTagKey(tag);
        RedisValue[] members = _database.SetMembers(tagKey);
        RedisKey[] keys = [.. members.Select(key => (RedisKey)key.ToString())];
        _database.KeyDelete(keys);
        _database.KeyDelete(tagKey);
    }

    public async Task InvalidateTagAsync(string tag)
    {
        string tagKey = CreateTagKey(tag);
        RedisValue[] members = await _database.SetMembersAsync(tagKey);
        RedisKey[] keys = [.. members.Select(key => (RedisKey)key.ToString())];
        await _database.KeyDeleteAsync(keys);
        await _database.KeyDeleteAsync(tagKey);
    }

    public void InvalidateTags(params string[] tags)
    {
        foreach (string tag in tags)
            InvalidateTag(tag);
    }

    public Task InvalidateTagsAsync(params string[] tags)
    {
        return Task.WhenAll(tags.Select(InvalidateTagAsync));
    }

    public bool Lock(string key, TimeSpan expiration)
    {
        string lockKey = CreateLockKey(key);
        bool isLocked = _database.LockTake(lockKey, lockKey, expiration);
        if (isLocked)
        {
            logger.LogInformation("Lock is acquired for key {Key}", key);
            return true;
        }

        logger.LogInformation("Lock is already acquired for key {Key}", key);
        return false;
    }

    public async Task<bool> LockAsync(string key, TimeSpan expiration)
    {
        string lockKey = CreateLockKey(key);
        bool isLocked = await _database.LockTakeAsync(lockKey, lockKey, expiration);
        if (isLocked)
        {
            logger.LogInformation("Lock is acquired for key {Key}", key);
            return true;
        }

        logger.LogInformation("Lock is already acquired for key {Key}", key);
        return false;
    }

    public void Remove(string key)
    {
        _database.KeyDelete(CreateKey(key));
    }

    public async Task RemoveByPatternAsync(string pattern)
    {
        string[] keys = await GetKeysByPatternAsync(pattern);
        RedisKey[] redisKeys = [.. keys.Select(key => (RedisKey)key)];
        await _database.KeyDeleteAsync(redisKeys);
    }

    public void RemoveKeys(string[] keys)
    {
        RedisKey[] redisKeys = [.. keys.Select(key => (RedisKey)CreateKey(key))];
        _database.KeyDelete(redisKeys);
    }

    public T Set<T>(string key, T value, TimeSpan? expiration, params string[] tags)
    {
        key = CreateKey(key);
        string json = value.ConvertToJson();
        _database.StringSet(key, json, expiration);
        if (tags.Length == 0)
            return value;

        foreach (string tag in tags)
        {
            string tagKey = CreateTagKey(tag);
            _database.SetAdd(tagKey, key);

            if (expiration.HasValue)
                _database.KeyExpire(tagKey, expiration);
        }

        return value;
    }

    public bool TryGet<T>(string key, out Maybe<T> value)
    {
        Maybe<T> maybe = Get<T>(key);
        value = maybe;
        return maybe.HasValue;
    }

    public bool Unlock(string key)
    {
        string lockKey = CreateLockKey(key);
        return _database.LockRelease(lockKey, lockKey);
    }

    public Task<bool> UnlockAsync(string key)
    {
        string lockKey = CreateLockKey(key);
        return _database.LockReleaseAsync(lockKey, lockKey);
    }

    private string CreateTagKey(string tag) => CreateKey(nameof(tag), tag);
    private string CreateLockKey(string key) => CreateKey(nameof(key), key);
    private string CreateKey(string prefix, string key) => $"{_serviceName}:{prefix}:{key}";
    private string CreateKey(string key) => $"{_serviceName}:{key}";

}

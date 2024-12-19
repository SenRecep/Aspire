using CSharpEssentials;

namespace CompanyName.BuildingBlocks.Caching.Base;
public interface ICacheService
{
    bool IsAvailable { get; }
    bool TryGet<T>(string key, out Maybe<T> value);
    Maybe<T> Get<T>(string key);
    T GetOrCreate<T>(string key, Func<T> valueFactory, TimeSpan? expiration, params string[] tags);
    Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> valueFactory, TimeSpan? expiration, params string[] tags);
    Task<string[]> GetKeysByPatternAsync(string pattern);

    T Set<T>(string key, T value, TimeSpan? expiration, params string[] tags);

    void Remove(string key);
    void RemoveKeys(string[] keys);
    Task RemoveByPatternAsync(string pattern);

    void InvalidateTag(string tag);
    void InvalidateTags(params string[] tags);

    Task InvalidateTagAsync(string tag);
    Task InvalidateTagsAsync(params string[] tags);

    bool Lock(string key, TimeSpan expiration);
    Task<bool> LockAsync(string key, TimeSpan expiration);

    bool Unlock(string key);
    Task<bool> UnlockAsync(string key);
}

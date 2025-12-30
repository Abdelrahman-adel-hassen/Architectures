namespace LearningJourney.Infrastructure.Caching;

public class MemoryCacheProvider(IMemoryCache cache) : ICacheProvider
{
    private readonly IMemoryCache _cache = cache;

    public Task<(bool Found, T Value)> TryGetAsync<T>(string key)
    {
        if (_cache.TryGetValue(key, out T value))
            return Task.FromResult((true, value));

        return Task.FromResult((false, default(T)));
    }

    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        _cache.TryGetValue(key, out T value);
        return Task.FromResult<T?>(value);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };

        _cache.Set(key, value, options);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        _cache.Remove(key);
        return Task.CompletedTask;
    }
}
namespace LearningJourney.Infrastructure.Caching
{
    public class RedisCacheProvider(IDistributedCache cache) : ICacheProvider
    {
        private readonly IDistributedCache _cache = cache;
        private readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web);

        public async Task<(bool Found, T Value)> TryGetAsync<T>(string key)
        {
            var bytes = await _cache.GetAsync(key);
            if (bytes == null || bytes.Length == 0)
                return (false, default);

            var json = Encoding.UTF8.GetString(bytes);
            var value = JsonSerializer.Deserialize<T>(json, _serializerOptions);
            return (true, value);
        }
        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var bytes = await _cache.GetAsync(key);
            if (bytes == null || bytes.Length == 0) 
                return default;
            return JsonSerializer.Deserialize<T>(bytes!);
        }
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var json = JsonSerializer.Serialize(value, _serializerOptions);
            var bytes = Encoding.UTF8.GetBytes(json);

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };

            await _cache.SetAsync(key, bytes, options);
        }

        public Task RemoveAsync(string key) => _cache.RemoveAsync(key);
    }
}
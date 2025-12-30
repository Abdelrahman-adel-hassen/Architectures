namespace LearningJourney.Application.Common.Behaviors.Caching
{
    public interface ICacheProvider
    {
        Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);
        Task<(bool Found, T Value)> TryGetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
        Task RemoveAsync(string key);
    }
}
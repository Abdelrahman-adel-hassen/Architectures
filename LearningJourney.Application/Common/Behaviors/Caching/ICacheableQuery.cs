namespace LearningJourney.Application.Common.Behaviors.Caching
{
    public interface ICacheableQuery
    {
        string CacheKey { get; }
        TimeSpan? Expiration { get; }
    }
}

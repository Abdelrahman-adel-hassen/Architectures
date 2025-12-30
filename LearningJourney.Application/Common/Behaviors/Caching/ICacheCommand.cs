namespace LearningJourney.Application.Common.Behaviors.Caching
{
    public interface ICacheCommand
    {
        string[] CacheKeys { get; }
    }
}

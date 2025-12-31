namespace LearningJourney.Infrastructure.Exceptions;
public class RedisNotFoundException() : Exception("Redis connection string is missing")
{
}

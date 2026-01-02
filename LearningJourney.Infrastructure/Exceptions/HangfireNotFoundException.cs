namespace LearningJourney.Infrastructure.Exceptions;

public class HangfireNotFoundException() : Exception("Hangfire connection string is missing")
{
}

namespace LearningJourney.Shared.Abstractions;

public abstract class Entity<T>
{
    public required T Id { get; set; }
}
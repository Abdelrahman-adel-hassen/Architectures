using LearningJourney.Shared.Abstractions;

namespace LearningJourney.Shared.Entities;

public class City : BaseEntity<Guid>
{
    public string Name { get; set; }

    public ICollection<Hospital> Hospitals { get; set; }
}

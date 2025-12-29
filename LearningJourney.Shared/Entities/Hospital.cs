namespace LearningJourney.Shared.Entities;

public class Hospital : BaseEntity<Guid>
{
    public string Name { get; set; }

    public ICollection<Doctor> Doctors { get; set; }
}

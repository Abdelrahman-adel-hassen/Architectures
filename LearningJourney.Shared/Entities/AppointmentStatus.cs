using LearningJourney.Shared.Abstractions;

namespace LearningJourney.Shared.Entities;

public class AppointmentStatus :  BaseEntity<Guid>
{
    public string Name { get; set; }

    public ICollection<Appointment> Appointments { get; set; }
}

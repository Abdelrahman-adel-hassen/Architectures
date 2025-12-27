using LearningJourney.Shared.Abstractions;

namespace LearningJourney.Shared.Entities;

public class AppointmentStatus :  BaseEntity<Guid>
{
    public string Name { get; set; } // Pending, Confirmed, Cancelled, Completed

    public ICollection<Appointment> Appointments { get; set; }
}

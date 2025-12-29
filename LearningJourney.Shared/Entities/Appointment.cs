using LearningJourney.Shared.Enums;

namespace LearningJourney.Shared.Entities;

public class Appointment : BaseEntity<Guid>
{
    public DateTime Date { get; set; }

    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }

    public Guid DoctorId { get; set; }
    public Doctor Doctor { get; set; }

    public AppointmentStatus Status { get; set; }
}

namespace LearningJourney.Shared.Entities;

public class Comment : BaseEntity<Guid>
{
    public string Text { get; set; }

    public Guid AppointmentId { get; set; }
    public Appointment Appointment { get; set; }
}

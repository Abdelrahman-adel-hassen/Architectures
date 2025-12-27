namespace LearningJourney.Shared.Entities;

public class Attachment : BaseEntity<Guid>
{
    public string FileName { get; set; }
    public string FilePath { get; set; }

    public Guid AppointmentId { get; set; }
    public Appointment Appointment { get; set; }
}

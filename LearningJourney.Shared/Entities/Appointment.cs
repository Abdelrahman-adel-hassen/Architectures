namespace LearningJourney.Shared.Entities;

public class Appointment : BaseEntity<Guid>
{
    public DateTime AppointmentDate { get; set; }

    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }

    public Guid DoctorId { get; set; }
    public Doctor Doctor { get; set; }

    public Guid ScheduleSlotId { get; set; }
    public ScheduleSlot ScheduleSlot { get; set; }

    public Guid AppointmentStatusId { get; set; }
    public AppointmentStatus AppointmentStatus { get; set; }

    public ICollection<Attachment> Attachments { get; set; }
    public ICollection<Comment> Comments { get; set; }
}

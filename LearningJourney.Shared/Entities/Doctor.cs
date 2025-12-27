using LearningJourney.Shared.Abstractions;

namespace LearningJourney.Shared.Entities;

public class Doctor : BaseEntity<Guid>
{
    public string FullName { get; set; }

    public Guid SpecialtyId { get; set; }

    public Guid HospitalId { get; set; }
    public Hospital Hospital { get; set; }

    public ICollection<ScheduleSlot> ScheduleSlots { get; set; }
    public ICollection<Appointment> Appointments { get; set; }
}

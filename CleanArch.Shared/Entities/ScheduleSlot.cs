using CleanArch.Shared.Abstractions;

namespace CleanArch.Shared.Entities;

public class ScheduleSlot : BaseEntity<Guid>
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public Guid DoctorId { get; set; }
    public Doctor Doctor { get; set; }

    public bool IsBooked { get; set; }
}

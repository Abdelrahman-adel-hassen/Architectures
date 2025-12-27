using LearningJourney.Shared.Abstractions;

namespace LearningJourney.Shared.Entities;

public class Customer : BaseEntity<Guid>
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }

    public ICollection<Appointment> Appointments { get; set; }
}

using LearningJourney.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearningJourney.Application.Common.Appstractions;

public interface ILearningJourneyContext
{
    DbSet<ScheduleSlot> ScheduleSlots { get; set; }
    DbSet<Appointment> Appointments { get; set; }
    DbSet<Customer> Customers { get; set; }
    DbSet<Doctor> Doctors { get; set; }
    DbSet<AppointmentStatus> AppointmentStatuses { get; set; }
    DbSet<Attachment> Attachments { get; set; }
    DbSet<Comment> Comments { get; set; }
    DbSet<Hospital> Hospitals { get; set; }
    DbSet<Owner> Owners { get; set; }
    DbSet<City> Cities { get; set; }

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

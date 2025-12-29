using LearningJourney.Shared.Enums;

namespace LearningJourney.Application.Common.Appstractions;

public interface ILearningJourneyContext
{
    DbSet<Appointment> Appointments { get; set; }
    DbSet<Customer> Customers { get; set; }
    DbSet<Doctor> Doctors { get; set; }
    DbSet<Hospital> Hospitals { get; set; }

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

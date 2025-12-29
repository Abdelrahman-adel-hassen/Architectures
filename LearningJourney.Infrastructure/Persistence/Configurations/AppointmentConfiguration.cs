using LearningJourney.Shared.Enums;

namespace LearningJourney.Infrastructure.Persistence.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Date)
               .IsRequired();

        builder.HasOne(x => x.Customer)
               .WithMany(c => c.Appointments)
               .HasForeignKey(x => x.CustomerId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Doctor)
               .WithMany(d => d.Appointments)
               .HasForeignKey(x => x.DoctorId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(a => a.Status)
                .HasConversion<int>()
                .IsRequired()
                .HasDefaultValue(AppointmentStatus.Pending);
    }
}
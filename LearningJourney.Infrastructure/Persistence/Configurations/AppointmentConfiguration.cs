using LearningJourney.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningJourney.Infrastructure.Persistence.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.AppointmentDate)
               .IsRequired();

        builder.HasOne(x => x.Customer)
               .WithMany(c => c.Appointments)
               .HasForeignKey(x => x.CustomerId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Doctor)
               .WithMany(d => d.Appointments)
               .HasForeignKey(x => x.DoctorId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ScheduleSlot)
               .WithMany()
               .HasForeignKey(x => x.ScheduleSlotId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.AppointmentStatus)
               .WithMany(s => s.Appointments)
               .HasForeignKey(x => x.AppointmentStatusId);
    }
}
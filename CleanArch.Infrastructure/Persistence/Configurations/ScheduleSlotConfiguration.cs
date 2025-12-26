using CleanArch.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArch.Infrastructure.Persistence.Configurations;

public class ScheduleSlotConfiguration : IEntityTypeConfiguration<ScheduleSlot>
{
    public void Configure(EntityTypeBuilder<ScheduleSlot> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.StartTime).IsRequired();
        builder.Property(x => x.EndTime).IsRequired();

        builder.HasOne(x => x.Doctor)
               .WithMany(d => d.ScheduleSlots)
               .HasForeignKey(x => x.DoctorId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

using LearningJourney.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningJourney.Infrastructure.Persistence.Configurations;

public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FileName).IsRequired();

        builder.HasOne(x => x.Appointment)
               .WithMany(a => a.Attachments)
               .HasForeignKey(x => x.AppointmentId);
    }
}

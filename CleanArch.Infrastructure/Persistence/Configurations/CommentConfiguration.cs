using CleanArch.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArch.Infrastructure.Persistence.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Text).IsRequired();

        builder.HasOne(x => x.Appointment)
               .WithMany(a => a.Comments)
               .HasForeignKey(x => x.AppointmentId);
    }
}

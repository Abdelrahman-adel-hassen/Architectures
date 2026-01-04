namespace LearningJourney.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.IdNumber)
               .IsRequired();

        builder.Property(x => x.PasswordHash)
               .IsRequired()
               .HasMaxLength(500);

        builder.Property(x => x.Email)
               .HasMaxLength(150);

        builder.Property(x => x.FirstName)
               .HasMaxLength(100);

        builder.Property(x => x.LastName)
               .HasMaxLength(100);

        builder.HasIndex(x => x.IdNumber)
               .IsUnique();
    }
}

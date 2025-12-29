namespace LearningJourney.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FullName)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(x => x.PhoneNumber)
               .HasMaxLength(20);

        builder.Property(x => x.Email)
               .HasMaxLength(150);
    }
}

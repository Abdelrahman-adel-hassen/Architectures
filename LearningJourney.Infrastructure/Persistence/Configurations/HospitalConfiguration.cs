namespace LearningJourney.Infrastructure.Persistence.Configurations;

public class HospitalConfiguration : IEntityTypeConfiguration<Hospital>
{
    public void Configure(EntityTypeBuilder<Hospital> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(200);
    }
}

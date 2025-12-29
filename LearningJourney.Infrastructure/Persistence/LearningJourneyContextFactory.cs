namespace LearningJourney.Infrastructure.Persistence;

public class LearningJourneyContextFactory : IDesignTimeDbContextFactory<LearningJourneyContext>
{
    public LearningJourneyContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
                         .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "LearningJourney.API"))
                         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                         .AddJsonFile($"appsettings.Development.json", optional: true)
                         .AddEnvironmentVariables()
                         .Build();

        var optionsBuilder = new DbContextOptionsBuilder<LearningJourneyContext>();

        optionsBuilder.UseSqlServer(
            configuration.GetConnectionString("DefaultConnection")
        );

        return new LearningJourneyContext(optionsBuilder.Options);
    }
}
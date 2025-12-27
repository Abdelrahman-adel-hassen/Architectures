namespace LearningJourney.Infrastructure;

public static class InfrastructureExtenstion
{
    public static IServiceProvider AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LearningJourneyContext>(options =>
        {
            options.UseSqlServer(
               configuration.GetConnectionString("DefaultConnection"),
               b => b.MigrationsAssembly(typeof(LearningJourneyContext).Assembly.FullName));
        });

        services.AddScoped<ILearningJourneyContext>(provider => provider.GetRequiredService<LearningJourneyContext>());

        return services.BuildServiceProvider();
    }
}

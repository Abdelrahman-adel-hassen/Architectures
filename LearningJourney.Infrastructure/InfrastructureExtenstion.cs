using LearningJourney.Infrastructure.Persistence.Seed;
using Microsoft.AspNetCore.Builder;
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
        services.AddScoped<IDataSeeder, DataSeeder>();


        return services.BuildServiceProvider();
    }
    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        // Configure the HTTP request pipeline.
        // 1. Use Api Endpoint services

        // 2. Use Application Use Case services

        // 3. Use Data - Infrastructure services
        UseMigration(app);

        return app;
    }
    private static IApplicationBuilder UseMigration(IApplicationBuilder app)
    {
        MigrateDatabaseAsync(app.ApplicationServices).GetAwaiter().GetResult();

        SeedDataAsync(app.ApplicationServices).GetAwaiter().GetResult();

        return app;
    }

    private static async Task MigrateDatabaseAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<LearningJourneyContext>();
        await context.Database.MigrateAsync();
    }

    private static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();
        foreach (var seeder in seeders)
        {
            await seeder.SeedAllAsync();
        }
    }
}

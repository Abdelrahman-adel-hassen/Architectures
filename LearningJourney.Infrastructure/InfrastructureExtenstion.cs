namespace LearningJourney.Infrastructure;

public static class InfrastructureExtenstion
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ILearningJourneyContext>(provider => provider.GetRequiredService<LearningJourneyContext>());
        services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
        services.AddHostedService<FileExportBackgroundService>();
        services.AddScoped<IDataSeeder, DataSeeder>();

        services.AddDbContext<LearningJourneyContext>(options =>
        {
            options.UseSqlServer(
               configuration.GetConnectionString("DefaultConnection"),
               b => b.MigrationsAssembly(typeof(LearningJourneyContext).Assembly.FullName));
        });

       
        var provider = configuration.GetValue<string>("Caching:Provider") ?? "Redis";

        if (string.Equals(provider, "Redis", StringComparison.OrdinalIgnoreCase))
        {
            var redisConnection = configuration.GetConnectionString("Redis") ?? throw new RedisNotFoundException();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnection;
            });

            services.AddSingleton<ICacheProvider, RedisCacheProvider>();
        }
        else
        {
            services.AddMemoryCache();
            services.AddSingleton<ICacheProvider, MemoryCacheProvider>();
        }


        return services;
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

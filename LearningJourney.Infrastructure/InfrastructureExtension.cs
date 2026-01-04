namespace LearningJourney.Infrastructure;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ILearningJourneyContext>(provider => provider.GetRequiredService<LearningJourneyContext>());
        services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
        services.AddScoped<IBackgroundJobScheduler, HangfireJobScheduler>();
        services.AddScoped<IExcelExporter, ClosedXmlExcelExporter>();
        services.AddHostedService<FileExportBackgroundService>();
        services.AddScoped<IDataSeeder, DataSeeder>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        
        string connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("DefaultConnection is not configured.");
       
        services.AddDbContext<LearningJourneyContext>(options =>
        {
            options.UseSqlServer(
               connectionString,
               b => b.MigrationsAssembly(typeof(LearningJourneyContext).Assembly.FullName));
        });

        AddCache(services, configuration);
        AddHangfireServices(services, configuration);
        return services;
    }

    private static void AddCache(IServiceCollection services, IConfiguration configuration)
    {
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
    }

    public static IServiceCollection AddHangfireServices(this IServiceCollection services, IConfiguration configuration)
    {
        var hangfireConnection = configuration.GetConnectionString("HangfireConnection") ?? throw new HangfireNotFoundException();

        services.AddHangfire(configuration =>
        {
            configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                         .UseSimpleAssemblyNameTypeSerializer()
                         .UseRecommendedSerializerSettings()
                         .UseSqlServerStorage(hangfireConnection);
        });

        services.AddHangfireServer();

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseHangfireDashboard("/Dashboard");
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

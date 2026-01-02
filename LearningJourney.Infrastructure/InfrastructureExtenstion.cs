namespace LearningJourney.Infrastructure;

public static class InfrastructureExtenstion
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ILearningJourneyContext>(provider => provider.GetRequiredService<LearningJourneyContext>());
        services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
        services.AddScoped<IBackgroundJobScheduler, HangfireJobScheduler>();
        services.AddScoped<IExcelExporter, ClosedXmlExcelExporter>();
        services.AddHostedService<FileExportBackgroundService>();
        services.AddScoped<IDataSeeder, DataSeeder>();
        
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
            //.UseSqlServerStorage(connectionString, new SqlServerStorageOptions
            //{
            //    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            //    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            //    QueuePollInterval = TimeSpan.Zero,
            //    UseRecommendedIsolationLevel = true,
            //    UsePageLocksOnDequeue = true,
            //    DisableGlobalLocks = true
            //});
        });

        services.AddHangfireServer();

        return services;
    }
    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        // Configure the HTTP request pipeline.
        // 1. Use Api Endpoint services

        // 2. Use Application Use Case services
        app.UseHangfireDashboard("/Dashboard");
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

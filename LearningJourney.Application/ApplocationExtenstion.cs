namespace LearningJourney.Application;

public static class ApplocationExtenstion
{
    public static IServiceCollection AddApplication(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(QueryCachingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CommandCacheBehavior<,>));
        services.Configure<ExportSettings>(configuration.GetSection("ExportSettings"));
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}

namespace LearningJourney.Application;

public static class ApplocationExtenstion
{
    public static IServiceProvider AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(QueryCachingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CommandCacheBehavior<,>));
        return services.BuildServiceProvider();
    }
}

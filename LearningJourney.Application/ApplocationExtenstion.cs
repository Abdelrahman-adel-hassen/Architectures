namespace LearningJourney.Application;

public static class ApplocationExtenstion
{
    public static IServiceProvider AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        return services.BuildServiceProvider();
    }
}

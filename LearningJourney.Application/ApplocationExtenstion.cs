namespace LearningJourney.Application;

public static class ApplocationExtenstion
{
    public static IServiceProvider AddApplication(this IServiceCollection services)
    {
        return services.BuildServiceProvider();
    }
}

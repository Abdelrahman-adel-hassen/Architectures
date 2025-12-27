using Microsoft.Extensions.DependencyInjection;

namespace LearningJourney.Application;

public static class ApplocationExtenstion
{
    public static IServiceProvider AddApplication(this IServiceCollection services)
    {
        //services.AddMediatR(Assembly.GetExecutingAssembly());

        return services.BuildServiceProvider();
    }
}

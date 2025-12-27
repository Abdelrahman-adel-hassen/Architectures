using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LearningJourney.Application;

public static class ApplocationExtenstion
{
    public static IServiceProvider AddApplication(this IServiceCollection services)
    {
        //services.AddMediatR(Assembly.GetExecutingAssembly());

        return services.BuildServiceProvider();
    }
}

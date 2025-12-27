using LearningJourney.Application.Common.Appstractions;
using LearningJourney.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

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

        services.AddScoped<ILearningJourneyContext>(provider => provider.GetRequiredService<ILearningJourneyContext>());

        return services.BuildServiceProvider();
    }
}

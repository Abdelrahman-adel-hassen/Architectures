using CleanArch.Application.Common.Appstractions;
using CleanArch.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CleanArch.Infrastructure;

public static class InfrastructureExtenstion
{
    public static IServiceProvider AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CleanArchContext>(options =>
        {
            options.UseSqlServer(
               configuration.GetConnectionString("DefaultConnection"),
               b => b.MigrationsAssembly(typeof(CleanArchContext).Assembly.FullName));
        });

        services.AddScoped<ICleanArchContext>(provider => provider.GetRequiredService<ICleanArchContext>());

        return services.BuildServiceProvider();
    }
}

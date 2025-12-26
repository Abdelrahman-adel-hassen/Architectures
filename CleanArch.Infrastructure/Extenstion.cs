using CleanArch.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArch.Infrastructure;

public static class Extenstion
{
    public static IServiceProvider AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CleanArchContext>(options =>
        {
            options.UseSqlServer(
               configuration.GetConnectionString("DefaultConnection"),
               b => b.MigrationsAssembly(typeof(CleanArchContext).Assembly.FullName));
        });
        return services.BuildServiceProvider();
    }
}

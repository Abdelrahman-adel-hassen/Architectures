using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CleanArch.Infrastructure.Persistence;

public class CleanArchContextFactory : IDesignTimeDbContextFactory<CleanArchContext>
{
    public CleanArchContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
                         .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "CleanArch.API"))
                         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                         .AddJsonFile($"appsettings.Development.json", optional: true)
                         .AddEnvironmentVariables()
                         .Build();

        var optionsBuilder = new DbContextOptionsBuilder<CleanArchContext>();

        optionsBuilder.UseSqlServer(
            configuration.GetConnectionString("DefaultConnection")
        );

        // 3️⃣ Return new context
        return new CleanArchContext(optionsBuilder.Options);
    }
}
namespace LearningJourney.Infrastructure.Persistence.Seed;
public class DataSeeder(LearningJourneyContext dbContext)
    : IDataSeeder
{
    public async Task SeedAllAsync()
    {
        if (!await dbContext.Hospitals.AnyAsync())
        {
            await dbContext.Hospitals.AddRangeAsync(InitialData.Hospitals);
            await dbContext.SaveChangesAsync();
        }

        if (!await dbContext.Doctors.AnyAsync())
        {
            await dbContext.Doctors.AddRangeAsync(InitialData.Doctors);
            await dbContext.SaveChangesAsync();
        }

        if (!await dbContext.Customers.AnyAsync())
        {
            await dbContext.Customers.AddRangeAsync(InitialData.Customers);
            await dbContext.SaveChangesAsync();
        }

        if (!await dbContext.Appointments.AnyAsync())
        { 
            await dbContext.Appointments.AddRangeAsync(InitialData.Appointments);
            await dbContext.SaveChangesAsync();
        }
    }
}

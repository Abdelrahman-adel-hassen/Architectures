namespace LearningJourney.Tests.Fixtures;

public class WebApplicationFactoryFixture : IAsyncLifetime
{
    private const string ConnectionString = @"Server=localhost\SQLEXPRESS;Database=LearningJourneyTest;Trusted_Connection=True;TrustServerCertificate=True";

    private WebApplicationFactory<Program> _factory;

    public HttpClient Client { get; private set; }
    public int InitialStudentsCount { get; set; } = 3;

    public WebApplicationFactoryFixture()
    {
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Test"); // 🔥 IMPORTANT

            builder.ConfigureTestServices(Services =>
            {
                Services.RemoveAll<DbContextOptions<LearningJourneyContext>>();
                Services.AddDbContext<LearningJourneyContext>(options =>
                {
                    options.UseSqlServer(ConnectionString);
                });
            });
        });
        Client = _factory.CreateClient();
    }
    public LearningJourneyContext CreateDbContext()
    {
        var scope = _factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<LearningJourneyContext>();
    }
    async Task IAsyncLifetime.DisposeAsync()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var cntx = scopedServices.GetRequiredService<LearningJourneyContext>();

            await cntx.Database.EnsureDeletedAsync();
        }
    }
    async Task IAsyncLifetime.InitializeAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var cntx = scopedServices.GetRequiredService<LearningJourneyContext>();

        await cntx.Database.EnsureCreatedAsync();

        // 1️⃣ Create hospitals
        var hospitals = DataFixture.GetHospitals(InitialStudentsCount);
        await cntx.Hospitals.AddRangeAsync(hospitals);

        // 2️⃣ Create doctors and assign them to existing hospitals
        var doctors = DataFixture.GetDoctors(InitialStudentsCount)
            .Select((d, i) =>
            {
                d.HospitalId = hospitals[i % hospitals.Count].Id;
                return d;
            })
            .ToList();
        await cntx.Doctors.AddRangeAsync(doctors);

        // 3️⃣ Create customers
        var customers = DataFixture.GetCustomers(InitialStudentsCount);
        await cntx.Customers.AddRangeAsync(customers);

        // 4️⃣ Create appointments using existing doctors and customers
        var appointments = new List<Appointment>();
        for (int i = 0; i < InitialStudentsCount; i++)
        {
            appointments.Add(new Appointment
            {
                Id = Guid.NewGuid(),
                CustomerId = customers[i % customers.Count].Id,
                DoctorId = doctors[i % doctors.Count].Id,
                Date = DateTime.UtcNow.AddDays(i) // or any date
            });
        }
        await cntx.Appointments.AddRangeAsync(appointments);

        await cntx.SaveChangesAsync();
    }

}

namespace LearningJourney.Tests.Fixtures
{
    public class DockerWebApplicationFactoryFixture : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private MsSqlContainer _dbContainer;
        public int InitialStudentsCount { get; } = 3;

        public DockerWebApplicationFactoryFixture()
        {
            _dbContainer = new MsSqlBuilder().Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var connectionString = _dbContainer.GetConnectionString();
            base.ConfigureWebHost(builder);
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<LearningJourneyContext>));
                services.AddDbContext<LearningJourneyContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });
            });
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();

            using (var scope = Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<LearningJourneyContext>();

                await cntx.Database.EnsureCreatedAsync();

                await cntx.Hospitals.AddRangeAsync(DataFixture.GetHospitals(InitialStudentsCount));
                await cntx.Doctors.AddRangeAsync(DataFixture.GetDoctors(InitialStudentsCount));
                await cntx.Customers.AddRangeAsync(DataFixture.GetCustomers(InitialStudentsCount));
                await cntx.Appointments.AddRangeAsync(DataFixture.GetAppointments(InitialStudentsCount));
                await cntx.SaveChangesAsync();
            }
        }

        public new async Task DisposeAsync()
        {
            await _dbContainer.StopAsync();
        }
    }
}

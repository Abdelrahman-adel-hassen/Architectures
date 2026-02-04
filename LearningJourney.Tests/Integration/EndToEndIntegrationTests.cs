namespace LearningJourney.Tests.Integration;

using LearningJourney.Tests.Helpers;

public class EndToEndIntegrationTests : IClassFixture<WebApplicationFactoryFixture>
{
    private readonly WebApplicationFactoryFixture _factory;
    private readonly HttpHelper _httpHelper;

    private Guid _hospitalId;
    private Guid _hospitalIdNumber;
    private const string HospitalPassword = "Hospital@123";
    private Guid _hospitalUserId;

    private Guid _customerId;
    private Guid _customerIdNumber;
    private const string CustomerPassword = "Customer@123";
    private Guid _customerUserId;

    private Guid _doctorId;

    public EndToEndIntegrationTests(WebApplicationFactoryFixture factory)
    {
        _factory = factory;
        _httpHelper = new HttpHelper(_factory.Client);
    }

    [Fact]
    public async Task FullFlowTest_RegisterLoginCreateDoctorCreateAppointment_Success()
    {
        // Step 1: Register Hospital User
        await RegisterHospitalUser();

        // Step 2: Login Hospital User and get token
        await LoginHospitalUser();

        // Step 5: Hospital creates a doctor
        await AddHospital();

        await HospitalCreateDoctor();

        // Step 3: Register Customer User
        await RegisterCustomerUser();

        // Step 4: Login Customer User and get token
        await LoginCustomerUser();

        // Step 6: Customer creates an appointment with the doctor
        await CustomerCreateAppointment();

        // Step 7: Verify appointment was created successfully
        await VerifyAppointmentCreated();
    }

    private async Task RegisterHospitalUser()
    {
        _hospitalIdNumber = Guid.NewGuid();
        var registerCommand = new RegisterCommand(
            IdNumber: _hospitalIdNumber,
            Password: HospitalPassword,
            UserType: UserType.Hospital,
            FirstName: "Hospital",
            LastName: "Admin",
            Email: "hospital@example.com",
            PhoneNumber: "+12345678901"
        );

        var response = await _httpHelper.PostWithResponseAsync(HttpHelper.Endpoints.Register, registerCommand);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var responseContent = await response.Content.ReadAsStringAsync();
        _hospitalUserId = HttpHelper.DeserializeJson<Guid>(responseContent);
        _hospitalUserId.Should().NotBe(Guid.Empty);
    }

    private async Task LoginHospitalUser()
    {

        var response = await _httpHelper.PostWithResponseAsync(HttpHelper.Endpoints.Login(_hospitalIdNumber), HospitalPassword);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var responseContent = await response.Content.ReadAsStringAsync();
        var token = HttpHelper.GetJsonProperty(responseContent, "token");
        token.Should().NotBeNullOrEmpty();

        // Set token for subsequent requests
        _httpHelper.SetBearerToken(token!);
    }

    private async Task RegisterCustomerUser()
    {
        _customerIdNumber = Guid.NewGuid();
        var registerCommand = new RegisterCommand(
            IdNumber: _customerIdNumber,
            Password: CustomerPassword,
            UserType: UserType.Customer,
            FirstName: "John",
            LastName: "Doe",
            Email: "customer@example.com",
            PhoneNumber: "+1234567890"
        );

        var response = await _httpHelper.PostWithResponseAsync(HttpHelper.Endpoints.Register, registerCommand);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var responseContent = await response.Content.ReadAsStringAsync();
        _customerUserId = HttpHelper.DeserializeJson<Guid>(responseContent);
        _customerUserId.Should().NotBe(Guid.Empty);
    }

    private async Task LoginCustomerUser()
    {
        var response = await _httpHelper.PostWithResponseAsync(HttpHelper.Endpoints.Login(_customerIdNumber), CustomerPassword);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var responseContent = await response.Content.ReadAsStringAsync();
        var token = HttpHelper.GetJsonProperty(responseContent, "token");
        token.Should().NotBeNullOrEmpty();

        // Set token for subsequent requests (customer context)
        _httpHelper.SetBearerToken(token!);
    }

    private async Task AddHospital()
    {
        // First, get or create a hospital from the fixture setup
        using var context = _factory.CreateDbContext();


        // If no hospital exists, create one in the database
        _hospitalId = Guid.NewGuid();
        var newHospital = new Hospital
        {
            Id = _hospitalId,
            Name = "Test Hospital",
            IdNumber = _hospitalIdNumber
        };
        context.Hospitals.Add(newHospital);
        await context.SaveChangesAsync();

    }

    private async Task HospitalCreateDoctor()
    {
        var createDoctorCommand = new
        {
            fullName = "Dr. Jane Smith",
            hospitalId = _hospitalId
        };

        var response = await _httpHelper.PostWithResponseAsync(HttpHelper.Endpoints.CreateDoctor, createDoctorCommand);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var responseContent = await response.Content.ReadAsStringAsync();
        _doctorId = HttpHelper.DeserializeJson<Guid>(responseContent);
        _doctorId.Should().NotBe(Guid.Empty);
    }

    private async Task CustomerCreateAppointment()
    {
        // Get the customer ID from the database
        Guid customerId;
        using var context = _factory.CreateDbContext();

        var customer = await context.Customers.FirstOrDefaultAsync();

        if (customer != null)
        {
            customerId = customer.Id;
        }
        else
        {
            // Create a customer in the database if needed
            customerId = Guid.NewGuid();
            var newCustomer = new Customer
            {
                Id = customerId,
                FullName = "John Doe",
                Email = "customer@example.com",
                PhoneNumber = "+1234567890"
            };
            context.Customers.Add(newCustomer);
            await context.SaveChangesAsync();
        }

        var createAppointmentCommand = new
        {
            customerId = customerId,
            doctorId = _doctorId,
            scheduleSlotId = Guid.NewGuid(),
            appointmentDate = DateTime.UtcNow.AddDays(1)
        };

        var response = await _httpHelper.PostWithResponseAsync(HttpHelper.Endpoints.CreateAppointment, createAppointmentCommand);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var responseContent = await response.Content.ReadAsStringAsync();
        var appointmentId = HttpHelper.DeserializeJson<Guid>(responseContent);
        appointmentId.Should().NotBe(Guid.Empty);
    }

    private async Task VerifyAppointmentCreated()
    {
        using var context = _factory.CreateDbContext();

        var appointments = await context.Appointments
            .Include(a => a.Doctor)
            .Include(a => a.Customer)
            .ToListAsync();

        appointments.Should().NotBeEmpty();
        var lastAppointment = appointments.Last();
        lastAppointment.DoctorId.Should().Be(_doctorId);
        lastAppointment.Doctor.Should().NotBeNull();
        lastAppointment.Customer.Should().NotBeNull();
    }
}

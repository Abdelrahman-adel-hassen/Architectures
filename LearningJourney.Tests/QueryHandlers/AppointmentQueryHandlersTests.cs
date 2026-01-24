namespace LearningJourney.Tests.QueryHandlers;

public class GetAppointmentByIdQueryHandlerTests
    : IClassFixture<WebApplicationFactoryFixture>
{
    private readonly WebApplicationFactoryFixture _factory;
    private readonly IMapper _mapper;

    public GetAppointmentByIdQueryHandlerTests(WebApplicationFactoryFixture factory)
    {
        _factory = factory;

        // real AutoMapper config (recommended for integration-style tests)
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(GetAppointmentByIdQueryHandler).Assembly);
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Handle_WithExistingAppointment_ReturnsAppointmentDto()
    {
        // Arrange
        using var context = _factory.CreateDbContext();

        var existingAppointment = await context.Appointments
            .Include(a => a.Customer)
            .Include(a => a.Doctor)
            .FirstAsync();

        var handler = new GetAppointmentByIdQueryHandler(context, _mapper);
        var query = new GetAppointmentByIdQuery(existingAppointment.Id);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(existingAppointment.Id);
        result.CustomerName.Should().Be(existingAppointment.Customer!.FullName);
        result.DoctorName.Should().Be(existingAppointment.Doctor!.FullName);
    }

    [Fact]
    public async Task Handle_WithNonExistentAppointment_ReturnsNull()
    {
        // Arrange
        using var context = _factory.CreateDbContext();
        var handler = new GetAppointmentByIdQueryHandler(context, _mapper);

        var query = new GetAppointmentByIdQuery(Guid.NewGuid());

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}

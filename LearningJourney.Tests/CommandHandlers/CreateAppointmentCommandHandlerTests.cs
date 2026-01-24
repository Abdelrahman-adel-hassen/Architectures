namespace LearningJourney.Tests.CommandHandlers;

public class CreateAppointmentCommandHandlerTests
{
    private readonly Mock<ILearningJourneyContext> _contextMock;
    private readonly CreateAppointmentCommandHandler _handler;

    public CreateAppointmentCommandHandlerTests()
    {
        _contextMock = new Mock<ILearningJourneyContext>();
        _handler = new CreateAppointmentCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CreatesAppointment()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var doctorId = Guid.NewGuid();
        var scheduleSlotId = Guid.NewGuid();
        var appointmentDate = DateTime.UtcNow.AddDays(1);

        var command = new CreateAppointmentCommand(
            CustomerId: customerId,
            DoctorId: doctorId,
            ScheduleSlotId: scheduleSlotId,
            AppointmentDate: appointmentDate
        );

        var appointmentsSet = new Mock<DbSet<Appointment>>();
        _contextMock.Setup(c => c.Appointments).Returns(appointmentsSet.Object);
        _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBe(Guid.Empty);
        appointmentsSet.Verify(a => a.Add(It.IsAny<Appointment>()), Times.Once);
        _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidCommand_AppointmentDataIsCorrect()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var doctorId = Guid.NewGuid();
        var scheduleSlotId = Guid.NewGuid();
        var appointmentDate = DateTime.UtcNow.AddDays(1);

        var command = new CreateAppointmentCommand(
            CustomerId: customerId,
            DoctorId: doctorId,
            ScheduleSlotId: scheduleSlotId,
            AppointmentDate: appointmentDate
        );

        Appointment? capturedAppointment = null;
        var appointmentsSet = new Mock<DbSet<Appointment>>();
        appointmentsSet.Setup(a => a.Add(It.IsAny<Appointment>())).Callback<Appointment>(a => capturedAppointment = a);

        _contextMock.Setup(c => c.Appointments).Returns(appointmentsSet.Object);
        _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedAppointment.Should().NotBeNull();
        capturedAppointment!.CustomerId.Should().Be(customerId);
        capturedAppointment.DoctorId.Should().Be(doctorId);
        capturedAppointment.Date.Should().Be(appointmentDate);
        capturedAppointment.CreatedBy.Should().Be("System");
        capturedAppointment.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task Handle_WithValidCommand_ReturnsNewAppointmentId()
    {
        // Arrange
        var command = new CreateAppointmentCommand(
            CustomerId: Guid.NewGuid(),
            DoctorId: Guid.NewGuid(),
            ScheduleSlotId: Guid.NewGuid(),
            AppointmentDate: DateTime.UtcNow.AddDays(1)
        );

        var appointmentsSet = new Mock<DbSet<Appointment>>();
        _contextMock.Setup(c => c.Appointments).Returns(appointmentsSet.Object);
        _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result1 = await _handler.Handle(command, CancellationToken.None);
        var result2 = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result1.Should().NotBe(result2);
        result1.Should().NotBe(Guid.Empty);
        result2.Should().NotBe(Guid.Empty);
    }
}

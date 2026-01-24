namespace LearningJourney.Tests.CommandHandlers;

public class CreateCustomerCommandHandlerTests
{
    private readonly Mock<ILearningJourneyContext> _contextMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly CreateCustomerCommandHandler _handler;

    public CreateCustomerCommandHandlerTests()
    {
        _contextMock = new Mock<ILearningJourneyContext>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _handler = new CreateCustomerCommandHandler(_contextMock.Object, _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CreatesCustomer()
    {
        // Arrange
        var command = new CreateCustomerCommand(
            FullName: "John Doe",
            Email: "john@example.com",
            PhoneNumber: "+1234567890"
        );

        _currentUserServiceMock.Setup(s => s.IsAuthenticated).Returns(true);
        _currentUserServiceMock.Setup(s => s.UserType).Returns(UserType.Customer);

        var customersSet = new Mock<DbSet<Customer>>();
        _contextMock.Setup(c => c.Customers).Returns(customersSet.Object);
        _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBe(Guid.Empty);
        customersSet.Verify(c => c.Add(It.IsAny<Customer>()), Times.Once);
        _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithUnauthenticatedUser_ThrowsUnauthorizedException()
    {
        // Arrange
        var command = new CreateCustomerCommand(
            FullName: "John Doe",
            Email: "john@example.com",
            PhoneNumber: "+1234567890"
        );

        _currentUserServiceMock.Setup(s => s.IsAuthenticated).Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithNonCustomerUserType_ThrowsUnauthorizedException()
    {
        // Arrange
        var command = new CreateCustomerCommand(
            FullName: "John Doe",
            Email: "john@example.com",
            PhoneNumber: "+1234567890"
        );

        _currentUserServiceMock.Setup(s => s.IsAuthenticated).Returns(true);
        _currentUserServiceMock.Setup(s => s.UserType).Returns(UserType.Doctor);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithValidCommand_CustomerDataIsCorrect()
    {
        // Arrange
        var command = new CreateCustomerCommand(
            FullName: "Jane Smith",
            Email: "jane@example.com",
            PhoneNumber: "+9876543210"
        );

        _currentUserServiceMock.Setup(s => s.IsAuthenticated).Returns(true);
        _currentUserServiceMock.Setup(s => s.UserType).Returns(UserType.Customer);

        Customer? capturedCustomer = null;
        var customersSet = new Mock<DbSet<Customer>>();
        customersSet.Setup(c => c.Add(It.IsAny<Customer>())).Callback<Customer>(c => capturedCustomer = c);

        _contextMock.Setup(c => c.Customers).Returns(customersSet.Object);
        _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedCustomer.Should().NotBeNull();
        capturedCustomer!.FullName.Should().Be("Jane Smith");
        capturedCustomer.Email.Should().Be("jane@example.com");
        capturedCustomer.PhoneNumber.Should().Be("+9876543210");
        capturedCustomer.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task Handle_WithAdminUserType_ThrowsUnauthorizedException()
    {
        // Arrange
        var command = new CreateCustomerCommand(
            FullName: "John Doe",
            Email: "john@example.com",
            PhoneNumber: "+1234567890"
        );

        _currentUserServiceMock.Setup(s => s.IsAuthenticated).Returns(true);
        _currentUserServiceMock.Setup(s => s.UserType).Returns(UserType.Admin);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() => _handler.Handle(command, CancellationToken.None));
    }
}

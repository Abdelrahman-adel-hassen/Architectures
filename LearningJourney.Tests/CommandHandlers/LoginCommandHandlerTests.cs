namespace LearningJourney.Tests.CommandHandlers
{
    public class LoginCommandHandlerTests
    {
        private ILearningJourneyContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<LearningJourneyContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new LearningJourneyContext(options);
        }

        [Fact]
        public async Task Handle_ValidUser_ShouldReturnLoginResponse()
        {
            // Arrange
            var context = CreateInMemoryContext();

            var password = "Password123";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                IdNumber = Guid.NewGuid(),
                PasswordHash = hashedPassword,
                Email = "john@example.com",
                UserType = UserType.Customer,
                FirstName = "John"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var jwtServiceMock = new Mock<IJwtService>();
            jwtServiceMock.Setup(j => j.GenerateToken(user.Id, user.IdNumber, user.FirstName, user.Email, user.UserType))
                          .Returns("FakeToken");

            var handler = new LoginCommandHandler(context, jwtServiceMock.Object);

            var command = new LoginCommand(user.IdNumber, password);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Token.Should().Be("FakeToken");
            result.UserId.Should().Be(user.Id);
            result.IdNumber.Should().Be(user.IdNumber);
            result.Email.Should().Be(user.Email);
            result.UserType.Should().Be(user.UserType);
        }

        [Fact]
        public async Task Handle_InvalidPassword_ShouldThrowNotFoundException()
        {
            // Arrange
            var context = CreateInMemoryContext();

            var user = new User
            {
                Id = Guid.NewGuid(),
                IdNumber = Guid.NewGuid(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("CorrectPassword"),
                UserType = UserType.Customer
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var jwtServiceMock = new Mock<IJwtService>();
            var handler = new LoginCommandHandler(context, jwtServiceMock.Object);

            var command = new LoginCommand(user.IdNumber, "WrongPassword");

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Invalid ID number or password");
        }

        [Fact]
        public async Task Handle_NonExistingUser_ShouldThrowNotFoundException()
        {
            // Arrange
            var context = CreateInMemoryContext();

            var jwtServiceMock = new Mock<IJwtService>();
            var handler = new LoginCommandHandler(context, jwtServiceMock.Object);

            var command = new LoginCommand(Guid.NewGuid(), "AnyPassword");

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Invalid ID number or password");
        }
    }
}

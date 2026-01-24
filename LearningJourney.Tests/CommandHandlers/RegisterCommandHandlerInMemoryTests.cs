namespace LearningJourney.Tests.CommandHandlers
{
    public class RegisterCommandHandlerInMemoryTests
    {
        private ILearningJourneyContext CreateInMemoryContext
        {
            get
            {
                var options = new DbContextOptionsBuilder<LearningJourneyContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;

                return new LearningJourneyContext(options);
            }
        }

        [Fact]
        public async Task Handle_NewUser_ShouldCreateUserAndReturnIdNumber()
        {
            // Arrange
            var context = CreateInMemoryContext;
            var handler = new RegisterCommandHandler(context);

            var command = new RegisterCommand(
                IdNumber: Guid.NewGuid(),
                Password: "Password123",
                UserType: UserType.Customer,
                FirstName: "John",
                LastName: "Doe",
                Email: "john@example.com",
                PhoneNumber: "+12345678901"
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(command.IdNumber);

            var userInDb = await context.Users.FirstOrDefaultAsync(u => u.IdNumber == command.IdNumber);
            userInDb.Should().NotBeNull();
            userInDb!.FirstName.Should().Be("John");
            userInDb.PasswordHash.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Handle_ExistingUser_ShouldThrowBadRequestException()
        {
            // Arrange
            var context = CreateInMemoryContext;

            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                IdNumber = Guid.NewGuid(),
                PasswordHash = "hashed",
                UserType = UserType.Customer
            };
            context.Users.Add(existingUser);
            await context.SaveChangesAsync();

            var handler = new RegisterCommandHandler(context);

            var command = new RegisterCommand(
                IdNumber: existingUser.IdNumber,
                Password: "Password123",
                UserType: UserType.Customer,
                FirstName: "John",
                LastName: "Doe",
                Email: "john@example.com",
                PhoneNumber: "+12345678901"
            );

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("User with this ID number already exists");
        }
    }
}

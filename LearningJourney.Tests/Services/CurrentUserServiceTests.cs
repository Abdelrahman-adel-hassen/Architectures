namespace LearningJourney.Tests.Services
{
    public class CurrentUserServiceTests
    {
        [Fact]
        public void Should_Return_UserId_When_ClaimExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"));

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(c => c.User).Returns(claimsPrincipal);

            var accessorMock = new Mock<IHttpContextAccessor>();
            accessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);

            var service = new CurrentUserService(accessorMock.Object);

            // Act
            var result = service.UserId;

            // Assert
            result.Should().Be(userId);
        }

        [Fact]
        public void Should_Return_Null_When_UserIdClaimMissing()
        {
            // Arrange
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(c => c.User).Returns(claimsPrincipal);

            var accessorMock = new Mock<IHttpContextAccessor>();
            accessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);

            var service = new CurrentUserService(accessorMock.Object);

            // Act
            var result = service.UserId;

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Should_Return_UserType_When_ClaimExists()
        {
            // Arrange
            var claims = new[]
            {
                new Claim("UserType", UserType.Customer.ToString())
            };
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"));

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(c => c.User).Returns(claimsPrincipal);

            var accessorMock = new Mock<IHttpContextAccessor>();
            accessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);

            var service = new CurrentUserService(accessorMock.Object);

            // Act
            var result = service.UserType;

            // Assert
            result.Should().Be(UserType.Customer);
        }

        [Fact]
        public void Should_Return_False_For_IsAuthenticated_When_NoUser()
        {
            // Arrange
            var accessorMock = new Mock<IHttpContextAccessor>();
            accessorMock.Setup(a => a.HttpContext).Returns((HttpContext)null);

            var service = new CurrentUserService(accessorMock.Object);

            // Act
            var result = service.IsAuthenticated;

            // Assert
            result.Should().BeFalse();
        }
    }
}

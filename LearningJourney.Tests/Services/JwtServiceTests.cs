namespace LearningJourney.Tests.Services
{
    public class JwtServiceTests
    {
        private readonly JwtService _jwtService;

        public JwtServiceTests()
        {
            var jwtSettings = Options.Create(new JwtSettings
            {
                SecretKey = "MySuperSecretKeyForTestingPurposes123!",
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                ExpirationInMinutes = 60
            });

            _jwtService = new JwtService(jwtSettings);
        }

        [Fact]
        public void GenerateToken_Should_Return_NonEmptyString()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var idNumber = Guid.NewGuid();
            var userName = "John Doe";
            var email = "john@example.com";
            var userType = UserType.Customer;

            // Act
            var token = _jwtService.GenerateToken(userId, idNumber, userName, email, userType);

            // Assert
            token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ValidateToken_Should_Return_True_For_ValidToken()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var idNumber = Guid.NewGuid();
            var userName = "John Doe";
            var email = "john@example.com";
            var userType = UserType.Customer;

            var token = _jwtService.GenerateToken(userId, idNumber, userName, email, userType);

            // Act
            var isValid = _jwtService.ValidateToken(token);

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void ValidateToken_Should_Return_False_For_InvalidToken()
        {
            // Arrange
            var invalidToken = "ThisIsNotAValidToken";

            // Act
            var isValid = _jwtService.ValidateToken(invalidToken);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void GenerateToken_Should_Contain_CorrectClaims()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var idNumber = Guid.NewGuid();
            var userName = "John Doe";
            var email = "john@example.com";
            var userType = UserType.Customer;

            // Act
            var tokenStr = _jwtService.GenerateToken(userId, idNumber, userName, email, userType);
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(tokenStr);
            // Assert
            jwt.Claims.Should().Contain(c => c.Type == "email" && c.Value == email);
            //jwt.Claims.Should().ContainSingle(c => c.Type == ClaimTypes.Name && c.Value == userName);
            jwt.Claims.Should().ContainSingle(c => c.Type == ClaimTypes.Sid && c.Value == idNumber.ToString());
            //jwt.Claims.Should().ContainSingle(c => c.Type == ClaimTypes.Email && c.Value == email);
            jwt.Claims.Should().Contain(c => c.Type == "role" && c.Value == "Customer");
            jwt.Claims.Should().ContainSingle(c => c.Type == "UserType" && c.Value == userType.ToString());
        }
    }
}

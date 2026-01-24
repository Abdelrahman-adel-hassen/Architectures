namespace LearningJourney.Tests.Helpers;

/// <summary>
/// Helper class to build test data for unit tests
/// </summary>
public static class TestDataBuilder
{
    public static Guid GenerateTestGuid() => Guid.NewGuid();

    public static string GenerateTestEmail() => $"test{Guid.NewGuid():N}@example.com";

    public static string GenerateTestUserName() => $"user_{Guid.NewGuid():N}".Substring(0, 20);

    public static Dictionary<string, object> CreateTestClaims(
        Guid userId,
        string userName,
        string email,
        string userType = "Customer")
    {
        return new Dictionary<string, object>
        {
            { "sub", userId.ToString() },
            { "name", userName },
            { "email", email },
            { "userType", userType }
        };
    }
}

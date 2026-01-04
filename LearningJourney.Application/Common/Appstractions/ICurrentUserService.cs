namespace LearningJourney.Application.Common.Appstractions;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    string? Username { get; }
    UserType? UserType { get; }
    bool IsAuthenticated { get; }
}

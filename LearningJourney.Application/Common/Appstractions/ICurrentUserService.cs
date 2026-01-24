namespace LearningJourney.Application.Common.Appstractions;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    Guid Sid { get; }
    string? Username { get; }
    UserType? UserType { get; }
    bool IsAuthenticated { get; }
}

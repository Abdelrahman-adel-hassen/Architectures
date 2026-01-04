using LearningJourney.Shared.Enums;

namespace LearningJourney.Application.Common.Appstractions;

public interface IJwtService
{
    string GenerateToken(Guid userId, Guid idNumber, string email, UserType userType);
    bool ValidateToken(string token);
}

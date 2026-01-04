using LearningJourney.Shared.Enums;

namespace LearningJourney.Shared.Entities;

public class User : BaseEntity<Guid>
{
    public Guid IdNumber { get; set; }
    public string PasswordHash { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public UserType UserType { get; set; }
}

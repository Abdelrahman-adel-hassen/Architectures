namespace LearningJourney.Shared.Abstractions;

public abstract class BaseEntity<T> : Entity<T>, IAuditable, ISoftDelete
{

    // Auditing
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; }

    public DateTime? LastModifiedAt { get; set; }
    public string LastModifiedBy { get; set; }

    // Soft delete
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    public string DeletedBy { get; set; }
}

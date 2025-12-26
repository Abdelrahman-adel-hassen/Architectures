namespace CleanArch.Shared.Abstractions;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }
    string DeletedBy { get; set; }
    DateTime? DeletedAt { get; set; }
}

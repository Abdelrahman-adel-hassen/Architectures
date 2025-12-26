using CleanArch.Shared.Abstractions;

namespace CleanArch.Shared.Entities;

public class Owner : BaseEntity<Guid>
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }

    public ICollection<Hospital> Hospitals { get; set; }
}

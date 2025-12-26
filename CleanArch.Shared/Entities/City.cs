using CleanArch.Shared.Abstractions;

namespace CleanArch.Shared.Entities;

public class City : BaseEntity<Guid>
{
    public string Name { get; set; }

    public ICollection<Hospital> Hospitals { get; set; }
}

using CleanArch.Shared.Abstractions;

namespace CleanArch.Shared.Entities;

public class Hospital : BaseEntity<Guid>
{
    public string Name { get; set; }

    public Guid CityId { get; set; }
    public City City { get; set; }

    public ICollection<Doctor> Doctors { get; set; }
}

namespace LearningJourney.Application.Features.Doctors.Queries;

public class DoctorDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public Guid SpecialtyId { get; set; }
    public Guid HospitalId { get; set; }
}

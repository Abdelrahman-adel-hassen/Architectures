namespace LearningJourney.Application.Features.Doctors.Queries;

public record GetDoctorsQuery() : IRequest<IEnumerable<DoctorDto>>;
public class GetDoctorsQueryHandler(ILearningJourneyContext context) : IRequestHandler<GetDoctorsQuery, IEnumerable<DoctorDto>>
{
    private readonly ILearningJourneyContext _context = context;

    public async Task<IEnumerable<DoctorDto>> Handle(GetDoctorsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Doctors
                             .AsNoTracking()
                             .Select(d => new DoctorDto
                             {
                                 Id = d.Id,
                                 FullName = d.FullName,
                                 HospitalId = d.HospitalId
                             })
                             .ToListAsync(cancellationToken);
    }
}
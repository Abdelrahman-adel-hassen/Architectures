namespace LearningJourney.Application.Features.Doctors.Queries;

public record GetDoctorByIdQuery(Guid Id) : IRequest<DoctorDto>;

public class GetDoctorByIdQueryHandler(ILearningJourneyContext context) : IRequestHandler<GetDoctorByIdQuery, DoctorDto>
{
    private readonly ILearningJourneyContext _context = context;

    public async Task<DoctorDto> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
    {
        var dto = await _context.Doctors
                                .AsNoTracking()
                                .Where(d => d.Id == request.Id)
                                .Select(d => new DoctorDto
                                {
                                    Id = d.Id,
                                    FullName = d.FullName,
                                    SpecialtyId = d.SpecialtyId,
                                    HospitalId = d.HospitalId
                                })
                                .SingleOrDefaultAsync(cancellationToken);

        return dto;
    }
}

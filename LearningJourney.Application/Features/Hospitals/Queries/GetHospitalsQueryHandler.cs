namespace LearningJourney.Application.Features.Hospitals.Queries;

public record GetHospitalsQuery() : IRequest<IEnumerable<HospitalDto>>;

public class GetHospitalsQueryHandler : IRequestHandler<GetHospitalsQuery, IEnumerable<HospitalDto>>
{
    private readonly ILearningJourneyContext _context;

    public GetHospitalsQueryHandler(ILearningJourneyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<HospitalDto>> Handle(GetHospitalsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Hospitals
                             .AsNoTracking()
                             .Select(h => new HospitalDto
                             {
                                 Id = h.Id,
                                 Name = h.Name,
                             })
                             .ToListAsync(cancellationToken);
    }
}
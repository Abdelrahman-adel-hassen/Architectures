namespace LearningJourney.Application.Features.Hospitals.Queries;

public record GetHospitalByIdQuery(Guid Id) : IRequest<HospitalDto>;
public class GetHospitalByIdQueryHandler(ILearningJourneyContext context) : IRequestHandler<GetHospitalByIdQuery, HospitalDto>
{
    private readonly ILearningJourneyContext _context = context;

    public async Task<HospitalDto> Handle(GetHospitalByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Hospitals
                             .AsNoTracking()
                             .Where(h => h.Id == request.Id)
                             .Select(h => new HospitalDto
                             {
                                 Id = h.Id,
                                 Name = h.Name,
                             })
                             .SingleOrDefaultAsync(cancellationToken);
    }
}

namespace LearningJourney.Application.Features.Doctors.Queries;

public record GetDoctorsQuery() : IRequest<IEnumerable<DoctorDto>>;
public class GetDoctorsQueryHandler(ILearningJourneyContext context,ICurrentUserService currentUserService) : IRequestHandler<GetDoctorsQuery, IEnumerable<DoctorDto>>
{

    public async Task<IEnumerable<DoctorDto>> Handle(GetDoctorsQuery request, CancellationToken cancellationToken)
    {
        var doctors= await context.Doctors
                             .AsNoTracking()
                             .Select(d => new DoctorDto
                             {
                                 Id = d.Id,
                                 FullName = d.FullName,
                                 HospitalId = d.HospitalId
                             })
                             .ToListAsync(cancellationToken);

        var idNumber = currentUserService.Sid == Guid.Empty
            ? throw new UnauthorizedAccessException("User is not authenticated.")
            : currentUserService.Sid;
        
        
        return doctors;
    }
}
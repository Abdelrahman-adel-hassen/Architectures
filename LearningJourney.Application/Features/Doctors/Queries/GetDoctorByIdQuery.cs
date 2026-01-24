namespace LearningJourney.Application.Features.Doctors.Queries;

public record GetDoctorByIdQuery(Guid Id) : IRequest<DoctorDto>;

public class GetDoctorByIdQueryHandler(ILearningJourneyContext context,ICurrentUserService currentUserService) : IRequestHandler<GetDoctorByIdQuery, DoctorDto>
{
    public async Task<DoctorDto> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
    {
        var dto = await context.Doctors
                                .AsNoTracking()
                                .Where(d => d.Id == request.Id)
                                .Select(d => new DoctorDto
                                {
                                    Id = d.Id,
                                    FullName = d.FullName,
                                    HospitalId = d.HospitalId,
                                    IdNumber =  d.IdNumber,
                                })
                                .SingleOrDefaultAsync(cancellationToken);
        if (dto == null)
            throw new KeyNotFoundException($"Doctor with id '{request.Id}' was not found.");
       
        var idNumber = currentUserService.Sid == Guid.Empty
            ? throw new UnauthorizedAccessException("User is not authenticated.")
            : currentUserService.Sid;

        return idNumber != dto.IdNumber ? throw new UnauthorizedAccessException("User is not authenticated.") : dto;
    }
}

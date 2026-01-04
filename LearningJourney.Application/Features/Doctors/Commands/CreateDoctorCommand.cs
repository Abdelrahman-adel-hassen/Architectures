namespace LearningJourney.Application.Features.Doctors.Commands;

public record CreateDoctorCommand(string FullName, Guid HospitalId) : IRequest<Guid>;

public class CreateDoctorCommandHandler(ILearningJourneyContext context, ICurrentUserService currentUserService) : IRequestHandler<CreateDoctorCommand, Guid>
{
    private readonly ILearningJourneyContext _context = context;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<Guid> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
    {
        // Check if user is authenticated
        if (!_currentUserService.IsAuthenticated)
        {
            throw new UnauthorizedException("User must be authenticated to create a doctor");
        }

        // Check if user has Doctor type
        if (_currentUserService.UserType != UserType.Doctor)
        {
            throw new UnauthorizedException("Only users with Doctor type can create doctors");
        }

        var entity = new Doctor
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            HospitalId = request.HospitalId
        };

        _context.Doctors.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
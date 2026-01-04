namespace LearningJourney.Application.Features.Hospitals.Commands;

public record CreateHospitalCommand(string Name, Guid CityId) : IRequest<Guid>;

public class CreateHospitalCommandHandler(ILearningJourneyContext context, ICurrentUserService currentUserService) : IRequestHandler<CreateHospitalCommand, Guid>
{
    private readonly ILearningJourneyContext _context = context;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<Guid> Handle(CreateHospitalCommand request, CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated)
        {
            throw new UnauthorizedException("User must be authenticated to create a hospital");
        }

        if (_currentUserService.UserType != UserType.Hospital)
        {
            throw new UnauthorizedException("Only users with Hospital type can create hospitals");
        }

        var entity = new Hospital
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
        };

        _context.Hospitals.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
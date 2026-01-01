namespace LearningJourney.Application.Features.Hospitals.Commands;

public record CreateHospitalCommand(string Name, Guid CityId) : IRequest<Guid>;

public class CreateHospitalCommandHandler : IRequestHandler<CreateHospitalCommand, Guid>
{
    private readonly ILearningJourneyContext _context;

    public CreateHospitalCommandHandler(ILearningJourneyContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateHospitalCommand request, CancellationToken cancellationToken)
    {
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
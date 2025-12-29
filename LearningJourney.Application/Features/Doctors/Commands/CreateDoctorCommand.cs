namespace LearningJourney.Application.Features.Doctors.Commands;

public record CreateDoctorCommand(string FullName, Guid HospitalId) : IRequest<Guid>;

public class CreateDoctorCommandHandler : IRequestHandler<CreateDoctorCommand, Guid>
{
    private readonly ILearningJourneyContext _context;

    public CreateDoctorCommandHandler(ILearningJourneyContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
    {
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
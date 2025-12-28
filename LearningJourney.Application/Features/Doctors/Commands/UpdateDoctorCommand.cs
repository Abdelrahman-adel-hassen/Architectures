namespace LearningJourney.Application.Features.Doctors.Commands;

public record UpdateDoctorCommand(Guid Id, string FullName, Guid SpecialtyId, Guid HospitalId) : IRequest<Unit>;

public class UpdateDoctorCommandHandler : IRequestHandler<UpdateDoctorCommand, Unit>
{
    private readonly ILearningJourneyContext _context;

    public UpdateDoctorCommandHandler(ILearningJourneyContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Doctors.SingleOrDefaultAsync(d => d.Id == request.Id, cancellationToken);
        if (entity == null)
            throw new KeyNotFoundException($"Doctor with id '{request.Id}' was not found.");

        entity.FullName = request.FullName;
        entity.SpecialtyId = request.SpecialtyId;
        entity.HospitalId = request.HospitalId;

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
namespace LearningJourney.Application.Features.Doctors.Commands;

public record DeleteDoctorCommand(Guid Id) : IRequest<Unit>;

public class DeleteDoctorCommandHandler : IRequestHandler<DeleteDoctorCommand, Unit>
{
    private readonly ILearningJourneyContext _context;

    public DeleteDoctorCommandHandler(ILearningJourneyContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteDoctorCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Doctors.SingleOrDefaultAsync(d => d.Id == request.Id, cancellationToken);
        if (entity == null)
            throw new KeyNotFoundException($"Doctor with id '{request.Id}' was not found.");

        _context.Doctors.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
namespace LearningJourney.Application.Features.Doctors.Commands;

public record DeleteDoctorCommand(Guid Id) : IRequest<Unit>;

public class DeleteDoctorCommandHandler(ILearningJourneyContext context, ICurrentUserService currentUserService)
    : IRequestHandler<DeleteDoctorCommand, Unit>
{
    public async Task<Unit> Handle(DeleteDoctorCommand request, CancellationToken cancellationToken)
    {
        var idNumber = currentUserService.Sid == Guid.Empty
            ? throw new UnauthorizedAccessException("User is not authenticated.")
            : currentUserService.Sid;
        
        if (idNumber == Guid.Empty)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var hospital = await context.Hospitals.FirstAsync(h => h.IdNumber == idNumber, cancellationToken);
        if (hospital.Doctors.All(x => x.Id != request.Id))
            throw new UnauthorizedAccessException("User is not authenticated.");

        var entity = await context.Doctors.SingleOrDefaultAsync(d => d.Id == request.Id, cancellationToken) ??
                     throw new KeyNotFoundException($"Doctor with id '{request.Id}' was not found.");

        context.Doctors.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
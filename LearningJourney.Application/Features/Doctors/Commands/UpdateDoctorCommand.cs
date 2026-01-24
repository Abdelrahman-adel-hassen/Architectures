namespace LearningJourney.Application.Features.Doctors.Commands;

public record UpdateDoctorCommand(Guid Id, string FullName, Guid HospitalId) : IRequest<Unit>;

public class UpdateDoctorCommandHandler(ILearningJourneyContext context,ICurrentUserService currentUserService) : IRequestHandler<UpdateDoctorCommand, Unit>
{
    public async Task<Unit> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
    {
        var idNumber = currentUserService.Sid == Guid.Empty
            ? throw new UnauthorizedAccessException("User is not authenticated.")
            : currentUserService.Sid;
        
        var entity = await context.Doctors.SingleOrDefaultAsync(d => d.Id == request.Id, cancellationToken);
        if (entity == null)
            throw new KeyNotFoundException($"Doctor with id '{request.Id}' was not found.");
        
        if (entity.IdNumber != idNumber)
            throw new UnauthorizedAccessException("User is not authenticated.");

        entity.FullName = request.FullName;
        entity.HospitalId = request.HospitalId;

        await context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
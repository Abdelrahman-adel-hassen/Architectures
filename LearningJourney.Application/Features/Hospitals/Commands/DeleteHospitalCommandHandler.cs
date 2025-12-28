using MediatR;
using LearningJourney.Application.Common.Appstractions;

namespace LearningJourney.Application.Features.Hospitals.Commands;

public record DeleteHospitalCommand(Guid Id) : IRequest<Unit>;

public class DeleteHospitalCommandHandler : IRequestHandler<DeleteHospitalCommand, Unit>
{
    private readonly ILearningJourneyContext _context;

    public DeleteHospitalCommandHandler(ILearningJourneyContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteHospitalCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Hospitals.SingleOrDefaultAsync(h => h.Id == request.Id, cancellationToken);
        if (entity == null)
            throw new KeyNotFoundException($"Hospital with id '{request.Id}' was not found.");

        _context.Hospitals.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
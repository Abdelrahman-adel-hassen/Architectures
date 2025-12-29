using MediatR;
using LearningJourney.Application.Common.Appstractions;
using LearningJourney.Shared.Entities;

namespace LearningJourney.Application.Features.Hospitals.Commands;

public record UpdateHospitalCommand(Guid Id, string Name, Guid CityId) : IRequest<Unit>;

public class UpdateHospitalCommandHandler : IRequestHandler<UpdateHospitalCommand, Unit>
{
    private readonly ILearningJourneyContext _context;

    public UpdateHospitalCommandHandler(ILearningJourneyContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateHospitalCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Hospitals.SingleOrDefaultAsync(h => h.Id == request.Id, cancellationToken);
        if (entity == null)
            throw new KeyNotFoundException($"Hospital with id '{request.Id}' was not found.");

        entity.Name = request.Name;

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
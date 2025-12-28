namespace LearningJourney.Application.Features.Customers.Commands;

public record UpdateCustomerCommand(Guid Id, string FirstName, string LastName, string Email) : IRequest<Unit>;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Unit>
{
    private readonly ILearningJourneyContext _context;

    public UpdateCustomerCommandHandler(ILearningJourneyContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Customers.SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (entity == null)
            throw new KeyNotFoundException($"Customer with id '{request.Id}' was not found.");

        entity.FullName = request.FirstName + " " + request.LastName;
        entity.Email = request.Email;

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
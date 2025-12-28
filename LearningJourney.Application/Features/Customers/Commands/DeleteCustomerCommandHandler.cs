namespace LearningJourney.Application.Features.Customers.Commands;

public record DeleteCustomerCommand(Guid Id) : IRequest<Unit>;

public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Unit>
{
    private readonly ILearningJourneyContext _context;

    public DeleteCustomerCommandHandler(ILearningJourneyContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Customers.SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (entity == null)
            throw new KeyNotFoundException($"Customer with id '{request.Id}' was not found.");

        _context.Customers.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
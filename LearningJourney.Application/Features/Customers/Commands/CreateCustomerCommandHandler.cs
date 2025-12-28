namespace LearningJourney.Application.Features.Customers.Commands;

public record CreateCustomerCommand(string FirstName, string LastName, string Email) : IRequest<Guid>;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
{
    private readonly ILearningJourneyContext _context;

    public CreateCustomerCommandHandler(ILearningJourneyContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = new Customer
        {
            Id = Guid.NewGuid(),
            FullName = request.FirstName + request.LastName,
            Email = request.Email
        };

        _context.Customers.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
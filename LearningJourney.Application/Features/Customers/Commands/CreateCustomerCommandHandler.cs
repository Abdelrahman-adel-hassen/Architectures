namespace LearningJourney.Application.Features.Customers.Commands;

public record CreateCustomerCommand(string FullName, string Email, string PhoneNumber) : IRequest<Guid>, ICacheCommand
{
    public string[] CacheKeys => ["Customers"];
}
public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email address");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^\+?\d{10,15}$").WithMessage("Invalid phone number format");
    }
}
public class CreateCustomerCommandHandler(ILearningJourneyContext context) : IRequestHandler<CreateCustomerCommand, Guid>
{
    private readonly ILearningJourneyContext _context = context;

    public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = new Customer
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };

        _context.Customers.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
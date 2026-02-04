namespace LearningJourney.Application.Features.Auth.Commands;

public record RegisterCommand(
    Guid IdNumber,
    string Password,
    UserType UserType,
    string? FirstName,
    string? LastName,
    string? Email = null,
    string? PhoneNumber = null)
    : IRequest<Guid>;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.IdNumber)
            .NotEmpty().WithMessage("ID number is required");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters");

        RuleFor(x => x.UserType)
            .IsInEnum().WithMessage("Invalid user type");

        RuleFor(x => x.FirstName)
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");

        RuleFor(x => x.LastName)
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");

        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("Invalid email address")
            .MaximumLength(150).WithMessage("Email cannot exceed 150 characters");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required")
            .Matches(@"^\+?\d{10,15}$")
            .WithMessage("Invalid phone number format");

        //// Validation for Doctor
        //RuleFor(x => x.FullName)
        //    .NotEmpty()
        //    .When(x => x.UserType == UserType.Doctor)
        //    .WithMessage("Full name is required for Doctor");
    }
}

public class RegisterCommandHandler(ILearningJourneyContext context) : IRequestHandler<RegisterCommand, Guid>
{
    private readonly ILearningJourneyContext _context = context;

    public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _context.Users.AnyAsync(u => u.IdNumber == request.IdNumber, cancellationToken))
        {
            throw new BadRequestException("User with this ID number already exists");
        }

        // Hash password
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            IdNumber = request.IdNumber,
            PasswordHash = passwordHash,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserType = request.UserType
        };

        _context.Users.Add(user);

        await _context.SaveChangesAsync(cancellationToken);

        return user.IdNumber;
    }
}

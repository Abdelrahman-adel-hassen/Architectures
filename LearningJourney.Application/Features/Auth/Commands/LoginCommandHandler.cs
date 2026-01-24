namespace LearningJourney.Application.Features.Auth.Commands;

public record LoginCommand(Guid IdNumber, string Password) : IRequest<LoginResponse>;

public record LoginResponse(string Token, Guid UserId, Guid IdNumber, string? Email, UserType UserType);

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.IdNumber)
            .NotEmpty().WithMessage("ID number is required");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}

public class LoginCommandHandler(ILearningJourneyContext context, IJwtService jwtService) : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly ILearningJourneyContext _context = context;
    private readonly IJwtService _jwtService = jwtService;

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.IdNumber == request.IdNumber, cancellationToken);

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new NotFoundException("Invalid ID number or password");
        
        var token = _jwtService.GenerateToken(user.Id, user.IdNumber,user.FirstName, user.Email ?? string.Empty, user.UserType);

        return new LoginResponse(token, user.Id, user.IdNumber, user.Email, user.UserType);

    }
}

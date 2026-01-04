namespace LearningJourney.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ISender mediator) : ControllerBase
{
    protected readonly ISender _mediator = mediator;

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterCommand command)
    {
        var IdNumber = await _mediator.Send(command);
        return CreatedAtAction(nameof(Login), new { IdNumber }, IdNumber);
    }

    [HttpPost("login/{IdNumber}")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(Guid IdNumber, [FromBody] string Password)
    {
        var result = await _mediator.Send(new LoginCommand(IdNumber, Password));
        return Ok(result);
    }
}

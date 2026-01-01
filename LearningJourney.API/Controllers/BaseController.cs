namespace LearningJourney.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController(ISender mediator) : ControllerBase
    {
        protected readonly ISender _mediator = mediator;
    }
}

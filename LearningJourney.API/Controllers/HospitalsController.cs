namespace LearningJourney.API.Controllers;
public class HospitalsController(ISender mediator) : BaseController(mediator)
{

    [HttpPost]
    public async Task<IActionResult> Create(CreateHospitalCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateHospitalCommand command)
    {
        if (id != command.Id) return BadRequest();
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [AdminOnly]
    //[Authorize(Policy = "AdminPolicy")] // using Policy in program
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteHospitalCommand(id));
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetHospitalByIdQuery(id));
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetHospitalsQuery());
        return Ok(result);
    }
}
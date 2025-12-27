using LearningJourney.Application.Commands.Appointments;
using LearningJourney.Application.Queries.Appointments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LearningJourney.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController(ISender mediator) : ControllerBase
{
    private readonly ISender _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create(CreateAppointmentCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateAppointmentCommand command)
    {
        if (id != command.Id) return BadRequest();
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteAppointmentCommand(id));
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetAppointmentByIdQuery(id));
        if (result == null) 
            return NotFound();
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAppointmentsQuery());
        return Ok(result);
    }
}

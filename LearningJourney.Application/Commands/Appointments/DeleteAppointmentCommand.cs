using LearningJourney.Application.Common.Appstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearningJourney.Application.Commands.Appointments;

public record DeleteAppointmentCommand(Guid Id) : IRequest<Unit>;

public class DeleteAppointmentCommandHandler(ILearningJourneyContext context) : IRequestHandler<DeleteAppointmentCommand,Unit>
{
    private readonly ILearningJourneyContext _context = context;

    public async Task<Unit> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        if (appointment == null) throw new KeyNotFoundException("Appointment not found");

        appointment.IsDeleted = true;
        appointment.DeletedAt = DateTime.UtcNow;
        appointment.DeletedBy = "System";

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

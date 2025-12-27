using LearningJourney.Application.Common.Appstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearningJourney.Application.Commands.Appointments;

public record UpdateAppointmentCommand(
    Guid Id,
    Guid CustomerId,
    Guid DoctorId,
    Guid ScheduleSlotId,
    DateTime AppointmentDate
) : IRequest<Unit>;

public class UpdateAppointmentCommandHandler(ILearningJourneyContext context) : IRequestHandler<UpdateAppointmentCommand,Unit>
{
    private readonly ILearningJourneyContext _context = context;

    public async Task<Unit> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        if (appointment == null) throw new KeyNotFoundException("Appointment not found");

        appointment.CustomerId = request.CustomerId;
        appointment.DoctorId = request.DoctorId;
        appointment.ScheduleSlotId = request.ScheduleSlotId;
        appointment.AppointmentDate = request.AppointmentDate;
        appointment.LastModifiedAt = DateTime.UtcNow;
        appointment.LastModifiedBy = "System";

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }

}
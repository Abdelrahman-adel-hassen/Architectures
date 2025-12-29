namespace LearningJourney.Application.Features.Appointments.Commands;

public record UpdateAppointmentCommand(
    Guid Id,
    Guid CustomerId,
    Guid DoctorId,
    Guid ScheduleSlotId,
    DateTime AppointmentDate
) : IRequest<Unit>;

public class UpdateAppointmentCommandHandler(ILearningJourneyContext context) : IRequestHandler<UpdateAppointmentCommand, Unit>
{
    private readonly ILearningJourneyContext _context = context;

    public async Task<Unit> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        if (appointment == null) throw new KeyNotFoundException("Appointment not found");

        appointment.CustomerId = request.CustomerId;
        appointment.DoctorId = request.DoctorId;
        appointment.Date = request.AppointmentDate;
        appointment.LastModifiedAt = DateTime.UtcNow;
        appointment.LastModifiedBy = "System";

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }

}
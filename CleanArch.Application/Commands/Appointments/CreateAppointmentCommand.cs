using CleanArch.Application.Common.Appstractions;
using CleanArch.Shared.Entities;
using MediatR;

namespace CleanArch.Application.Commands.Appointments;

public record CreateAppointmentCommand(
    Guid CustomerId,
    Guid DoctorId,
    Guid ScheduleSlotId,
    DateTime AppointmentDate
) : IRequest<Guid>;
public class CreateAppointmentCommandHandler(ICleanArchContext context) : IRequestHandler<CreateAppointmentCommand, Guid>
{
    private readonly ICleanArchContext _context = context;

    public async Task<Guid> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = new Appointment
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            DoctorId = request.DoctorId,
            ScheduleSlotId = request.ScheduleSlotId,
            AppointmentDate = request.AppointmentDate,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System" 
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync(cancellationToken);

        return appointment.Id;
    }
}

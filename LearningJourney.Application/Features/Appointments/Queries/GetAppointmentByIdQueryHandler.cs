namespace LearningJourney.Application.Features.Appointments.Queries;

public record GetAppointmentByIdQuery(Guid Id) : IRequest<AppointmentDto?>;
public class AppointmentDto
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; }
    public string DoctorName { get; set; }
    public DateTime AppointmentDate { get; set; }
}

public class GetAppointmentByIdQueryHandler(ILearningJourneyContext context) : IRequestHandler<GetAppointmentByIdQuery, AppointmentDto?>
{
    private readonly ILearningJourneyContext _context = context;

    public async Task<AppointmentDto?> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
    {
        var appointment = await _context.Appointments
            .Include(a => a.Customer)
            .Include(a => a.Doctor)
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (appointment == null) throw new KeyNotFoundException("Appointment not found");

        return new AppointmentDto
        {
            Id = appointment.Id,
            CustomerName = appointment.Customer.FullName,
            DoctorName = appointment.Doctor.FullName,
            AppointmentDate = appointment.Date
        };
    }
}
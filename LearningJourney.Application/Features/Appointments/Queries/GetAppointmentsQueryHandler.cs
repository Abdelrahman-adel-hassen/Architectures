namespace LearningJourney.Application.Features.Appointments.Queries;

public record GetAppointmentsQuery() : IRequest<List<GetAppointmentDto>>;
public class GetAppointmentDto
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; }
    public string DoctorName { get; set; }
    public DateTime AppointmentDate { get; set; }
}

public class GetAppointmentsQueryHandler(ILearningJourneyContext context) : IRequestHandler<GetAppointmentsQuery, List<GetAppointmentDto>>
{
    private readonly ILearningJourneyContext _context = context;

    public async Task<List<GetAppointmentDto>> Handle(GetAppointmentsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Appointments
            .Include(a => a.Customer)
            .Include(a => a.Doctor)
            .Select(a => new GetAppointmentDto
            {
                Id = a.Id,
                CustomerName = a.Customer.FullName,
                DoctorName = a.Doctor.FullName,
                AppointmentDate = a.AppointmentDate
            })
            .ToListAsync(cancellationToken);
    }
}
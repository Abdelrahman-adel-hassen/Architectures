using LearningJourney.Application.Common.Appstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearningJourney.Application.Queries.Appointments;

public record GetAppointmentsQuery() : IRequest<List<AppointmentDto>>;
public class GetAppointmentsQueryHandler(ILearningJourneyContext context) : IRequestHandler<GetAppointmentsQuery, List<AppointmentDto>>
{
    private readonly ILearningJourneyContext _context = context;

    public async Task<List<AppointmentDto>> Handle(GetAppointmentsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Appointments
            .Include(a => a.Customer)
            .Include(a => a.Doctor)
            .Select(a => new AppointmentDto
            {
                Id = a.Id,
                CustomerName = a.Customer.FullName,
                DoctorName = a.Doctor.FullName,
                AppointmentDate = a.AppointmentDate
            })
            .ToListAsync(cancellationToken);
    }
}
using CleanArch.Application.Common.Appstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace CleanArch.Application.Queries.Appointments;

public record GetAppointmentByIdQuery(Guid Id) : IRequest<AppointmentDto?>;
public class AppointmentDto
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; }
    public string DoctorName { get; set; }
    public DateTime AppointmentDate { get; set; }
}

public class GetAppointmentByIdQueryHandler(ICleanArchContext context) : IRequestHandler<GetAppointmentByIdQuery, AppointmentDto?>
{
    private readonly ICleanArchContext _context = context;

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
            AppointmentDate = appointment.AppointmentDate
        };
    }
}
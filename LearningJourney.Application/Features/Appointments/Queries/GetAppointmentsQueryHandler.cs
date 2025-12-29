using AutoMapper;
using LearningJourney.Shared.Entities;

namespace LearningJourney.Application.Features.Appointments.Queries;

public record GetAppointmentsQuery() : IRequest<List<GetAppointmentDto>>;
public class GetAppointmentDto : IMapFrom<Appointment>
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; }
    public string DoctorName { get; set; }
    public DateTime AppointmentDate { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Appointment, GetAppointmentDto>()
               .ForMember(d => d.CustomerName, opt => opt.MapFrom(s => s.Customer.FullName))
               .ForMember(d => d.DoctorName, opt => opt.MapFrom(s => s.Doctor.FullName))
               .ForMember(d => d.AppointmentDate, opt => opt.MapFrom(s => s.Date));
    }
}

public class GetAppointmentsQueryHandler(ILearningJourneyContext context, IMapper mapper) : IRequestHandler<GetAppointmentsQuery, List<GetAppointmentDto>>
{
    private readonly ILearningJourneyContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<List<GetAppointmentDto>> Handle(GetAppointmentsQuery request, CancellationToken cancellationToken)
    {
        var appointments =  await _context.Appointments
            .Include(a => a.Customer)
            .Include(a => a.Doctor)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<GetAppointmentDto>>(appointments); // bad use ProjectTo better
    }
}
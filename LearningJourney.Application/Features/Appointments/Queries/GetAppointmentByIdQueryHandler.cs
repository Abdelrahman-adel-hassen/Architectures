using AutoMapper.QueryableExtensions;

namespace LearningJourney.Application.Features.Appointments.Queries;

public record GetAppointmentByIdQuery(Guid Id) : IRequest<AppointmentDto?>;

public class AppointmentDto : IMapFrom<Appointment>
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; }
    public string DoctorName { get; set; }
    public DateTime AppointmentDate { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Appointment, AppointmentDto>()
               .ForMember(d => d.CustomerName, opt => opt.MapFrom(s => s.Customer.FullName))
               .ForMember(d => d.DoctorName, opt => opt.MapFrom(s => s.Doctor.FullName))
               .ForMember(d => d.AppointmentDate, opt => opt.MapFrom(s => s.Date));
    }
}

public class GetAppointmentByIdQueryHandler(ILearningJourneyContext context, IMapper mapper) : IRequestHandler<GetAppointmentByIdQuery, AppointmentDto?>
{
    private readonly ILearningJourneyContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<AppointmentDto?> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Appointments
           .Where(a => a.Id == request.Id)
           .ProjectTo<AppointmentDto>(_mapper.ConfigurationProvider)
           .FirstOrDefaultAsync(cancellationToken);
    }
}
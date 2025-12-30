namespace LearningJourney.Application.Features.Customers.Queries
{
    public record GetCustomersQuery() : IRequest<IEnumerable<CustomerDto>>, ICacheableQuery
    {
        public string CacheKey => $"Appointment:{typeof(GetCustomersQuery)}";
        public TimeSpan? Expiration => TimeSpan.FromMinutes(1);
    }

    public class GetCustomersQueryHandler(ILearningJourneyContext context, IMapper mapper) : IRequestHandler<GetCustomersQuery, IEnumerable<CustomerDto>>
    {
        private readonly ILearningJourneyContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            return await _context.Customers
                                 .AsNoTracking()
                                 .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
                                 .ToListAsync(cancellationToken);
        }
    }
}
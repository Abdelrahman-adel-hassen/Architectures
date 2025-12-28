namespace LearningJourney.Application.Features.Customers.Queries
{
    public record GetCustomersQuery() : IRequest<IEnumerable<CustomerDto>>;
    public class GetCustomersQueryHandler(ILearningJourneyContext context) : IRequestHandler<GetCustomersQuery, IEnumerable<CustomerDto>>
    {
        private readonly ILearningJourneyContext _context = context;

        public async Task<IEnumerable<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            return await _context.Customers
                                 .AsNoTracking()
                                 .Select(c => new CustomerDto
                                 {
                                     Id = c.Id,
                                     FullName = c.FullName,
                                     PhoneNumber = c.PhoneNumber,
                                     Email = c.Email
                                 })
                                 .ToListAsync(cancellationToken);
        }
    }
}
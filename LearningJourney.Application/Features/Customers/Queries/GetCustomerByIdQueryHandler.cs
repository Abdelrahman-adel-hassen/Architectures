namespace LearningJourney.Application.Features.Customers.Queries
{
    public record GetCustomerByIdQuery(Guid Id) : IRequest<CustomerDto?>, ICacheableQuery
    {
        public string CacheKey => $"CustomerById_{Id}";
        public TimeSpan? Expiration => TimeSpan.FromMinutes(1);
    }

    public class GetCustomerByIdQueryHandler(ILearningJourneyContext context) : IRequestHandler<GetCustomerByIdQuery, CustomerDto?>
    {
        private readonly ILearningJourneyContext _context = context;

        public async Task<CustomerDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Customers
                                 .AsNoTracking()
                                 .Where(c => c.Id == request.Id)
                                 .Select(c => new CustomerDto
                                 {
                                     Id = c.Id,
                                     FullName = c.FullName,
                                     PhoneNumber = c.PhoneNumber,
                                     Email = c.Email
                                 })
                                 .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
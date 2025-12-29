using LearningJourney.Application.Features.Appointments.Queries;

namespace LearningJourney.Application.Features.Customers.Queries
{
    public class CustomerDto : IMapFrom<Customer>
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Customer, CustomerDto>()
                   .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.FullName))
                   .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(s => s.PhoneNumber))
                   .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email));
        }
    }
}
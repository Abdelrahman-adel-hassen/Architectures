namespace LearningJourney.Tests.Fixtures
{
    internal class DataFixture
    {
        public static List<Hospital> GetHospitals(int count, bool useNewSeed = false)
        {
            return GetHospitalFaker(useNewSeed).Generate(count);
        }
        public static Hospital GetHospital(bool useNewSeed = false)
        {
            return GetHospitals(1, useNewSeed)[0];
        }

        private static Faker<Hospital> GetHospitalFaker(bool useNewSeed)
        {
            var seed = 0;
            if (useNewSeed)
            {
                seed = Random.Shared.Next(10, int.MaxValue);
            }
            return new Faker<Hospital>()
                .RuleFor(t => t.Id, o => Guid.NewGuid())
                .RuleFor(t => t.Name, (faker, t) => faker.Name.FullName())
                .RuleFor(t => t.IdNumber, (faker, t) => Guid.NewGuid())
                .UseSeed(seed);
        }
        public static List<Doctor> GetDoctors(int count, bool useNewSeed = false)
        {
            return GetDoctorFaker(useNewSeed).Generate(count);
        }
        public static Doctor GetDoctor(bool useNewSeed = false)
        {
            return GetDoctors(1, useNewSeed)[0];
        }

        private static Faker<Doctor> GetDoctorFaker(bool useNewSeed)
        {
            var seed = 0;
            if (useNewSeed)
            {
                seed = Random.Shared.Next(10, int.MaxValue);
            }
            return new Faker<Doctor>()
                .RuleFor(t => t.Id, o => Guid.NewGuid())
                .RuleFor(t => t.FullName, (faker, t) => faker.Name.FullName())
                .RuleFor(t => t.IdNumber, (faker, t) => Guid.NewGuid())
                .RuleFor(t => t.HospitalId, (faker, t) => Guid.Empty)
                .UseSeed(seed);
        }
        public static List<Customer> GetCustomers(int count, bool useNewSeed = false)
        {
            return GetCustomerFaker(useNewSeed).Generate(count);
        }
        public static Customer GetCustomer(bool useNewSeed = false)
        {
            return GetCustomers(1, useNewSeed)[0];
        }

        private static Faker<Customer> GetCustomerFaker(bool useNewSeed)
        {
            var seed = 0;
            if (useNewSeed)
            {
                seed = Random.Shared.Next(10, int.MaxValue);
            }
            return new Faker<Customer>()
                .RuleFor(t => t.Id, o => Guid.NewGuid())
                .RuleFor(t => t.FullName, (faker, t) => faker.Name.FullName())
                .RuleFor(t => t.Email, (faker, t) => faker.Internet.Email())
                .RuleFor(t => t.PhoneNumber, (faker, t) => faker.Phone.PhoneNumber())
                .UseSeed(seed);
        }
        public static List<Appointment> GetAppointments(int count, bool useNewSeed = false)
        {
            return GetAppointmentFaker(useNewSeed).Generate(count);
        }
        public static Appointment GetAppointment(bool useNewSeed = false)
        {
            return GetAppointments(1, useNewSeed)[0];
        }

        private static Faker<Appointment> GetAppointmentFaker(bool useNewSeed)
        {
            var seed = 0;
            if (useNewSeed)
            {
                seed = Random.Shared.Next(10, int.MaxValue);
            }
            return new Faker<Appointment>()
                .RuleFor(t => t.Id, o => Guid.NewGuid())
                .RuleFor(t => t.DoctorId, (faker, t) => Guid.Empty)
                .RuleFor(t => t.CustomerId, (faker, t) => Guid.Empty)
                .RuleFor(t => t.Date, (faker, t) => faker.Date.Past(5, new DateTime(2020, 1, 1)))
                .UseSeed(seed);
        }

    }
}

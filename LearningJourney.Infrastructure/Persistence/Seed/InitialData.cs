using LearningJourney.Shared.Enums;
namespace LearningJourney.Infrastructure.Persistence.Seed;

public static class InitialData
{
    public static IEnumerable<Hospital> Hospitals =>
    [
        new Hospital
        {
            Id = new Guid("a1b2c3d4-0000-4000-8000-000000000001"),
            Name = "Central City Hospital",
            CreatedAt = DateTime.Now,
            CreatedBy = "System"
        },
        new Hospital
        {
            Id = new Guid("a1b2c3d4-0000-4000-8000-000000000002"),
            Name = "Northside Medical Center",
            CreatedAt = DateTime.Now,
            CreatedBy = "System"
        }
    ];

    public static IEnumerable<Doctor> Doctors =>
    [
        new Doctor
        {
            Id = new Guid("b1b2c3d4-0000-4000-8000-000000000011"),
            FullName = "Alice Thompson",
            HospitalId = new Guid("a1b2c3d4-0000-4000-8000-000000000001"),
            CreatedAt = DateTime.Now,
            CreatedBy = "System"
        },
        new Doctor
        {
            Id = new Guid("b1b2c3d4-0000-4000-8000-000000000012"),
            FullName = "Michael Reed",
            HospitalId = new Guid("a1b2c3d4-0000-4000-8000-000000000002"),
            CreatedAt = DateTime.Now,
            CreatedBy = "System"
        }
    ];

    public static IEnumerable<Customer> Customers =>
    [
        new Customer
        {
            Id = new Guid("c1c2c3d4-0000-4000-8000-000000000021"),
            Email = "john.doe@example.com",
            FullName= "John Doe",
            PhoneNumber = "555-0301",
            CreatedAt = DateTime.Now,
            CreatedBy = "System"
        },
        new Customer
        {
            Id = new Guid("c1c2c3d4-0000-4000-8000-000000000022"),
            Email = "sara.kim@example.com",
            FullName = "Ms. Sara Kim",
            PhoneNumber = "555-0302",
            CreatedAt = DateTime.Now,
            CreatedBy = "System"
        }
    ];

    public static IEnumerable<Appointment> Appointments =>
    [
        new Appointment
        {
            Id = new Guid("d1d2d3d4-0000-4000-8000-000000000031"),
            Date = DateTime.UtcNow.Date.AddDays(3).AddHours(9),
            CustomerId = new Guid("c1c2c3d4-0000-4000-8000-000000000021"),
            DoctorId = new Guid("b1b2c3d4-0000-4000-8000-000000000011"),
            Status = AppointmentStatus.Pending,
            CreatedAt = DateTime.Now,
            CreatedBy = "System"
        },
        new Appointment
        {
            Id = new Guid("d1d2d3d4-0000-4000-8000-000000000032"),
            Date = DateTime.UtcNow.Date.AddDays(7).AddHours(14),
            CustomerId = new Guid("c1c2c3d4-0000-4000-8000-000000000022"),
            DoctorId = new Guid("b1b2c3d4-0000-4000-8000-000000000012"),
            Status = AppointmentStatus.Completed,
            CreatedAt = DateTime.Now,
            CreatedBy = "System"
        }
    ];
}
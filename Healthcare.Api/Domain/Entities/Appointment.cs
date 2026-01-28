
using Healthcare.Api.Domain.Enums;

namespace Healthcare.Api.Domain.Entities;

//public class Appointment
//{
//    public Guid Id { get; set; }
//    public Guid DoctorId { get; set; }
//    public Guid PatientId { get; set; }
//    public DateTime StartTimeUtc { get; set; }
//    public DateTime EndTimeUtc { get; set; }
//    public AppointmentStatus Status { get; set; }
//    public bool IsCancelled { get; set; }
//}

public class Appointment
{
    public Guid Id { get; set; }
    public Guid DoctorId { get; set; }
    public Guid PatientId { get; set; }
    public DateTimeOffset Start { get; set; }
    //public DateTime Start { get; set; }
    public int DurationMinutes { get; set; }
}

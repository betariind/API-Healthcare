
namespace Healthcare.Api.DTOs;

public class CreateAppointmentRequest
{
    public Guid DoctorId { get; set; }
    public Guid PatientId { get; set; }
    public DateTime StartUtc { get; set; }
    public int DurationMinutes { get; set; }
}

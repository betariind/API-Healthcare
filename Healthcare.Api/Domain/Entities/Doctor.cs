
namespace Healthcare.Api.Domain.Entities;

//public class Doctor
//{
//    public Guid Id { get; set; }
//    public string TimeZone { get; set; } = "Asia/Jakarta";
//    public ICollection<DoctorSchedule> Schedules { get; set; } = new List<DoctorSchedule>();
//}

public class Doctor
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}

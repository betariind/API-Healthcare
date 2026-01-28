
namespace Healthcare.Api.Domain.Entities;

//public class DoctorSchedule
//{
//    public Guid Id { get; set; }
//    public Guid DoctorId { get; set; }
//    public DayOfWeek DayOfWeek { get; set; }
//    public TimeSpan StartTime { get; set; }
//    public TimeSpan EndTime { get; set; }
//}

//public class DoctorSchedule
//{
//    public Guid Id { get; set; }
//    public Guid DoctorId { get; set; }

//    // RRule sederhana: "MO,WE,FR"
//    public string Days { get; set; } = "MO";

//    public TimeSpan StartTime { get; set; } // 09:00
//    public TimeSpan EndTime { get; set; }   // 12:00

//    // Parsing RRule ke DayOfWeek
//    public IEnumerable<DayOfWeek> ParsedDays =>
//        Days.Split(',', StringSplitOptions.RemoveEmptyEntries)
//            .Select(d => d.Trim() switch
//            {
//                "MO" => DayOfWeek.Monday,
//                "TU" => DayOfWeek.Tuesday,
//                "WE" => DayOfWeek.Wednesday,
//                "TH" => DayOfWeek.Thursday,
//                "FR" => DayOfWeek.Friday,
//                "SA" => DayOfWeek.Saturday,
//                "SU" => DayOfWeek.Sunday,
//                _ => throw new InvalidOperationException($"Invalid RRule day: {d}")
//            });
//}

public class RecurringSchedule
{
    public Guid Id { get; set; }
    public Guid DoctorId { get; set; }
    public int Day { get; set; } // 1 = Monday
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}

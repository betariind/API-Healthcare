
using Healthcare.Api.DTOs;
using Healthcare.Api.Domain.Enums;
using Healthcare.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Healthcare.Api.Services;

public class AvailabilityService
{
    private readonly HealthcareDbContext _db;
    public AvailabilityService(HealthcareDbContext db) => _db = db;

    public async Task<List<AvailabilitySlotDto>> GetAvailability(
    Guid doctorId,
    DateTime from,
    DateTime to,
    int slotMinutes)
    {
        // 1️⃣ VALIDASI SLOT
        if (slotMinutes != 15 && slotMinutes != 30 && slotMinutes != 60)
            throw new ArgumentException("Slot must be 15, 30, or 60 minutes");

        // pastikan UTC
        from = DateTime.SpecifyKind(from, DateTimeKind.Utc);
        to = DateTime.SpecifyKind(to, DateTimeKind.Utc);

        var schedules = await _db.RecurringSchedules
            .Where(s => s.DoctorId == doctorId)
            .ToListAsync();

        var appointments = await _db.Appointments
            .Where(a => a.DoctorId == doctorId)
            .ToListAsync();

        var result = new List<AvailabilitySlotDto>();

        for (var date = from.Date; date <= to.Date; date = date.AddDays(1))
        {
            var day = (int)date.DayOfWeek;
            if (day == 0) day = 7;

            foreach (var s in schedules.Where(x => x.Day == day))
            {
                // 2️⃣ schedule window
                var scheduleStart = date.Add(s.StartTime);
                var scheduleEnd = date.Add(s.EndTime);

                // 3️⃣ clamp ke from–to
                var windowStart = scheduleStart < from ? from : scheduleStart;
                var windowEnd = scheduleEnd > to ? to : scheduleEnd;

                // kalau window tidak valid, skip
                if (windowStart >= windowEnd)
                    continue;

                // 4️⃣ rounding ke kelipatan 5 menit
                windowStart = RoundToFiveMinutes(windowStart);

                var cursor = windowStart;

                while (cursor.AddMinutes(slotMinutes) <= windowEnd)
                {
                    var slotEnd = cursor.AddMinutes(slotMinutes);

                    // 5️⃣ overlap check
                    var busy = appointments.Any(a =>
                        cursor < a.Start.AddMinutes(a.DurationMinutes) &&
                        slotEnd > a.Start
                    );

                    if (!busy)
                    {
                        result.Add(new AvailabilitySlotDto
                        {
                            StartUtc = cursor,
                            EndUtc = slotEnd
                        });
                    }

                    cursor = cursor.AddMinutes(slotMinutes);
                }
            }
        }

        return result;
    }

    private static DateTime RoundToFiveMinutes(DateTime dt)
    {
        var minutes = (dt.Minute / 5) * 5;
        return new DateTime(
            dt.Year,
            dt.Month,
            dt.Day,
            dt.Hour,
            minutes,
            0,
            DateTimeKind.Utc
        );
    }


}

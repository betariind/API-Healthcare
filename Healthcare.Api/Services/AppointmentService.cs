using Healthcare.Api.Domain.Entities;
using Healthcare.Api.Domain.Enums;
using Healthcare.Api.DTOs;
using Healthcare.Api.Helpers;
using Healthcare.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using static Healthcare.Api.Controllers.AppointmentsController;

namespace Healthcare.Api.Services;

//public class AppointmentService
//{
//    private readonly HealthcareDbContext _db;
//    private const int CancellationCutoffHours = 2;

//    public AppointmentService(HealthcareDbContext db) => _db = db;

//    public async Task CreateAppointment(CreateAppointmentRequest req)
//    {
//        using var tx = await _db.Database.BeginTransactionAsync();

//        // 1️⃣ ROUNDING KE 5 MENIT
//        var startUtc = TimeHelper.RoundToFiveMinutes(req.StartUtc);
//        var endUtc = startUtc.AddMinutes(req.DurationMinutes);

//        // 2️⃣ AMBIL DOKTER + SCHEDULE + TIMEZONE
//        var doctor = await _db.Doctors
//            .Include(d => d.Schedules)
//            .FirstOrDefaultAsync(d => d.Id == req.DoctorId);

//        if (doctor == null)
//            throw new InvalidOperationException("Doctor not found");

//        var tz = TimeZoneInfo.FindSystemTimeZoneById(doctor.TimeZone);

//        // 3️⃣ VALIDASI JAM KERJA (RRule: MO,WE,FR)
//        var localTime = TimeZoneInfo.ConvertTimeFromUtc(startUtc, tz);

//        var isValidWorkingHour = doctor.Schedules.Any(s =>
//            s.ParsedDays.Contains(localTime.DayOfWeek) &&
//            localTime.TimeOfDay >= s.StartTime &&
//            localTime.TimeOfDay.Add(TimeSpan.FromMinutes(req.DurationMinutes)) <= s.EndTime
//        );

//        if (!isValidWorkingHour)
//            throw new InvalidOperationException("Outside working hours");

//        // 4️⃣ CEK OVERLAP (SETELAH JAM KERJA VALID)
//        var overlap = await _db.Appointments.AnyAsync(a =>
//            a.DoctorId == req.DoctorId &&
//            a.Status == AppointmentStatus.Active &&
//            startUtc < a.EndTimeUtc &&
//            endUtc > a.StartTimeUtc);

//        if (overlap)
//            throw new InvalidOperationException("Overlap appointment");

//        // 5️⃣ SIMPAN APPOINTMENT
//        _db.Appointments.Add(new Appointment
//        {
//            Id = Guid.NewGuid(),
//            DoctorId = req.DoctorId,
//            PatientId = req.PatientId,
//            StartTimeUtc = startUtc,
//            EndTimeUtc = endUtc,
//            Status = AppointmentStatus.Active
//        });

//        await _db.SaveChangesAsync();
//        await tx.CommitAsync();
//    }

//    public async Task CancelAppointment(Guid appointmentId)
//    {
//        var appt = await _db.Appointments.FindAsync(appointmentId);

//        if (appt == null || appt.Status == AppointmentStatus.Cancelled)
//            throw new InvalidOperationException();

//        // 6️⃣ CUTOFF 2 JAM
//        if ((appt.StartTimeUtc - DateTime.UtcNow).TotalHours < CancellationCutoffHours)
//            throw new InvalidOperationException("Cutoff exceeded");

//        appt.Status = AppointmentStatus.Cancelled;
//        await _db.SaveChangesAsync();
//    }
//}

public class AppointmentService
{
    private readonly HealthcareDbContext _db;

    public AppointmentService(HealthcareDbContext db)
    {
        _db = db;
    }


    public async Task CreateAppointmentAsync(CreateAppointmentRequest req)
    {
        using var tx = await _db.Database.BeginTransactionAsync(
            System.Data.IsolationLevel.Serializable);

        var start = new DateTimeOffset(req.StartUtc, TimeSpan.Zero);
        var end = start.AddMinutes(req.DurationMinutes);

        var startUtc = start.UtcDateTime;
        var endUtc = end.UtcDateTime;

        var day = (int)startUtc.DayOfWeek;
        if (day == 0) day = 7;

        var validSchedule = await _db.RecurringSchedules.AnyAsync(s =>
            s.DoctorId == req.DoctorId &&
            s.Day == day &&
            startUtc.TimeOfDay >= s.StartTime &&
            endUtc.TimeOfDay <= s.EndTime
        );

        if (!validSchedule)
            throw new InvalidOperationException("Out of schedule");

        var overlap = await _db.Appointments.AnyAsync(a =>
            a.DoctorId == req.DoctorId &&
            start < a.Start.AddMinutes(a.DurationMinutes) &&
            end > a.Start
        );

        if (overlap)
            throw new InvalidOperationException("Overlap");

        _db.Appointments.Add(new Appointment
        {
            Id = Guid.NewGuid(),
            DoctorId = req.DoctorId,
            PatientId = req.PatientId,
            Start = start,
            DurationMinutes = req.DurationMinutes
        });

        await _db.SaveChangesAsync();
        await tx.CommitAsync();
    }



    public async Task<CancelResult> CancelAppointment(Guid id, DateTimeOffset currentTime)
    {
        var appt = await _db.Appointments.FindAsync(id);
        if (appt == null)
            return CancelResult.NotFound;

        var cutOffTime = appt.Start.AddHours(-1);
        Console.WriteLine(currentTime);


        if (currentTime >= cutOffTime)
            return CancelResult.PassedCutOff;

        _db.Appointments.Remove(appt);
        await _db.SaveChangesAsync();

        return CancelResult.Success;
    }



}


using Healthcare.Api.Domain.Entities;
using Healthcare.Api.Domain.Enums;
using Healthcare.Api.DTOs;
using Healthcare.Api.Helpers;
using Healthcare.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
using static Healthcare.Api.Controllers.AppointmentsController;
=======
>>>>>>> 713f481ed8cb755fb512fcca86f8f7eaf97263f7

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

<<<<<<< HEAD

    public async Task CreateAppointmentAsync(CreateAppointmentRequest req)
    {
        using var tx = await _db.Database.BeginTransactionAsync(
            System.Data.IsolationLevel.Serializable);

        var start = new DateTimeOffset(req.StartUtc, TimeSpan.Zero);
        var end = start.AddMinutes(req.DurationMinutes);

=======
    //public async Task CreateAppointmentAsync(CreateAppointmentRequest req)
    //{
    //    // 1️⃣ Validasi pakai LOCAL TIME (schedule = local)
    //    var wibTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
    //    // Linux/Mac: "Asia/Jakarta"

    //    var localStart = TimeZoneInfo.ConvertTimeFromUtc(req.StartUtc, wibTimeZone);
    //    var localEnd = localStart.AddMinutes(req.DurationMinutes);


    //    // 2️⃣ Ambil semua schedule dokter
    //    var schedules = await _db.DoctorSchedules
    //        .Where(s => s.DoctorId == req.DoctorId)
    //        .ToListAsync();

    //    // 3️⃣ Cocokkan hari + jam praktik
    //    var isAvailable = schedules.Any(s =>
    //        s.ParsedDays.Contains(localStart.DayOfWeek) &&
    //        localStart.TimeOfDay >= s.StartTime &&
    //        localEnd.TimeOfDay <= s.EndTime
    //    );

    //    if (!isAvailable)
    //        throw new InvalidOperationException("Doctor is not available at this time");

    //    // 4️⃣ Hitung UTC untuk overlap check & save
    //    var endUtc = req.StartUtc.AddMinutes(req.DurationMinutes);

    //    // 5️⃣ Cek overlap appointment dokter
    //    var doctorOverlap = await _db.Appointments.AnyAsync(a =>
    //        a.DoctorId == req.DoctorId &&
    //        req.StartUtc < a.EndTimeUtc &&
    //        endUtc > a.StartTimeUtc
    //    );

    //    if (doctorOverlap)
    //        throw new InvalidOperationException("Doctor already has an appointment at this time");

    //    // 6️⃣ (Opsional) Cek overlap patient
    //    var patientOverlap = await _db.Appointments.AnyAsync(a =>
    //        a.PatientId == req.PatientId &&
    //        req.StartUtc < a.EndTimeUtc &&
    //        endUtc > a.StartTimeUtc
    //    );

    //    if (patientOverlap)
    //        throw new InvalidOperationException("Patient already has an appointment at this time");

    //    // 7️⃣ Simpan appointment (UTC)
    //    var appointment = new Appointment
    //    {
    //        Id = Guid.NewGuid(),
    //        DoctorId = req.DoctorId,
    //        PatientId = req.PatientId,
    //        StartTimeUtc = req.StartUtc,
    //        EndTimeUtc = endUtc
    //    };

    //    _db.Appointments.Add(appointment);
    //    await _db.SaveChangesAsync();
    //}

    //public async Task CreateAppointmentAsync(CreateAppointmentRequest req)
    //{
    //    var start = DateTime.SpecifyKind(req.StartUtc, DateTimeKind.Utc);
    //    var end = start.AddMinutes(req.DurationMinutes);

    //    var day = (int)start.DayOfWeek;
    //    if (day == 0) day = 7;

    //    var validSchedule = await _db.RecurringSchedules.AnyAsync(s =>
    //        s.DoctorId == req.DoctorId &&
    //        s.Day == day &&
    //        start.TimeOfDay >= s.StartTime &&
    //        end.TimeOfDay <= s.EndTime
    //    );

    //    if (!validSchedule)
    //        throw new Exception("Doctor is not available at the selected time");

    //    var overlap = await _db.Appointments.AnyAsync(a =>
    //        a.DoctorId == req.DoctorId &&
    //        start < a.Start.AddMinutes(a.DurationMinutes) &&
    //        end > a.Start
    //    );

    //    if (overlap)
    //        throw new Exception("Doctor already has appointment at the selected time");

    //    _db.Appointments.Add(new Appointment
    //    {
    //        Id = Guid.NewGuid(),
    //        DoctorId = req.DoctorId,
    //        PatientId = req.PatientId,
    //        Start = start,
    //        DurationMinutes = req.DurationMinutes
    //    });

    //    await _db.SaveChangesAsync();
    //}

    public async Task CreateAppointmentAsync(CreateAppointmentRequest req)
    {
        // convert request ke DateTimeOffset (UTC)
        var start = new DateTimeOffset(req.StartUtc, TimeSpan.Zero);
        var end = start.AddMinutes(req.DurationMinutes);

        // pakai DateTime UTC untuk logic hari & jam
>>>>>>> 713f481ed8cb755fb512fcca86f8f7eaf97263f7
        var startUtc = start.UtcDateTime;
        var endUtc = end.UtcDateTime;

        var day = (int)startUtc.DayOfWeek;
        if (day == 0) day = 7;

<<<<<<< HEAD
=======
        // cek jadwal dokter
>>>>>>> 713f481ed8cb755fb512fcca86f8f7eaf97263f7
        var validSchedule = await _db.RecurringSchedules.AnyAsync(s =>
            s.DoctorId == req.DoctorId &&
            s.Day == day &&
            startUtc.TimeOfDay >= s.StartTime &&
            endUtc.TimeOfDay <= s.EndTime
        );

        if (!validSchedule)
<<<<<<< HEAD
            throw new InvalidOperationException("Out of schedule");

=======
            throw new Exception("Doctor is not available at the selected time");

        // cek overlap
>>>>>>> 713f481ed8cb755fb512fcca86f8f7eaf97263f7
        var overlap = await _db.Appointments.AnyAsync(a =>
            a.DoctorId == req.DoctorId &&
            start < a.Start.AddMinutes(a.DurationMinutes) &&
            end > a.Start
        );

        if (overlap)
<<<<<<< HEAD
            throw new InvalidOperationException("Overlap");
=======
            throw new Exception("Doctor already has appointment at the selected time");
>>>>>>> 713f481ed8cb755fb512fcca86f8f7eaf97263f7

        _db.Appointments.Add(new Appointment
        {
            Id = Guid.NewGuid(),
            DoctorId = req.DoctorId,
            PatientId = req.PatientId,
<<<<<<< HEAD
            Start = start,
=======
            Start = start, // ⬅️ DateTimeOffset masuk DB
>>>>>>> 713f481ed8cb755fb512fcca86f8f7eaf97263f7
            DurationMinutes = req.DurationMinutes
        });

        await _db.SaveChangesAsync();
<<<<<<< HEAD
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



=======
    }


    //public async Task CancelAppointment(Guid id)
    //{
    //    var appointment = await _db.Appointments.FindAsync(id);

    //    if (appointment == null)
    //        throw new InvalidOperationException("Appointment not found");

    //    appointment.IsCancelled = true;
    //    await _db.SaveChangesAsync();
    //}

    public async Task<bool> CancelAppointment(Guid id)
    {
        var appt = await _db.Appointments.FindAsync(id);
        if (appt == null)
            return false;

        _db.Appointments.Remove(appt);
        await _db.SaveChangesAsync();
        return true;
    }

>>>>>>> 713f481ed8cb755fb512fcca86f8f7eaf97263f7
}


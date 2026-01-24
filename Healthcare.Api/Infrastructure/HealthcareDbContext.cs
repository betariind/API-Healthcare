
using Healthcare.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Healthcare.Api.Infrastructure;

public class HealthcareDbContext : DbContext
{
    public HealthcareDbContext(DbContextOptions options) : base(options) {}

    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<RecurringSchedule> RecurringSchedules => Set<RecurringSchedule>();
    public DbSet<Appointment> Appointments => Set<Appointment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>()
            .HasIndex(a => new { a.DoctorId, a.Start });
    }

}

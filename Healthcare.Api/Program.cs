using Healthcare.Api.Infrastructure;
using Healthcare.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔥 INI YANG DIGANTI (WAJIB)
builder.Services.AddDbContext<HealthcareDbContext>(opt =>
    opt.UseSqlServer(
        "Server=LAPTOP-IBJSCV3D\\SQLEXPRESS;Database=DoctorAppointmentDb;Trusted_Connection=True;TrustServerCertificate=True"
    ));

builder.Services.AddScoped<AvailabilityService>();
builder.Services.AddScoped<AppointmentService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

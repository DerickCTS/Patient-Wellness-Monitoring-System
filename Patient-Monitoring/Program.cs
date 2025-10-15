using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Patient_Monitoring.Data;
using Patient_Monitoring.Repository.Implementation;
using Patient_Monitoring.Repository.Interfaces;
using Patient_Monitoring.Jobs;

using Patient_Monitoring.Repositories.Interfaces;
using Patient_Monitoring.Repositories.Implementation;
using Patient_Monitoring.Services.Implementations;
using Patient_Monitoring.Services.Interfaces;



var builder = WebApplication.CreateBuilder(args);

const string AllowSpecificOrigins = "_allowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSpecificOrigins,
                      policy =>
                      {
                          // WARNING: AllowAnyOrigin is for TESTING/DEVELOPMENT only.
                          policy.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

// 🔹 Add controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Patient Monitoring API",
        Version = "v1"
    });
});

// 🔹 Configure Entity Framework
builder.Services.AddDbContext<PatientMonitoringDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PatientMonitoringDbContext")
        ?? throw new InvalidOperationException("Connection string 'PatientMonitoringDbContext' not found.")));

// 🔹 Register repositories and services
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ScheduledNotificationJob>();

// 🔹 Configure Hangfire
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("PatientMonitoringDbContext")));
builder.Services.AddHangfireServer();

builder.Services.AddScoped<IDoctorService, DoctorService>();

builder.Services.AddScoped<IWellnessPlanService, WellnessPlanService>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IWellnessPlanRepository, WellnessPlanRepository>();

var app = builder.Build();

// 🔹 Enable Hangfire dashboard
app.UseHangfireDashboard();

// 🔹 Register recurring jobs
RecurringJob.AddOrUpdate<ScheduledNotificationJob>(
    "medicine-reminders", job => job.SendMedicineRemindersAsync(), Cron.Minutely);

RecurringJob.AddOrUpdate<ScheduledNotificationJob>(
    "appointment-reminders", job => job.SendAppointmentRemindersAsync(), Cron.Daily);

RecurringJob.AddOrUpdate<ScheduledNotificationJob>(
    "daily-wellness-reminders", job => job.SendDailyWellnessRemindersAsync(), Cron.Daily(8));

// 🔹 Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
DataSeeder.Seed(app);

//DataSeeder.Seed(app);

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Patient Monitoring API v1");
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// 🔹 Map API routes
app.MapControllers();

app.Run();

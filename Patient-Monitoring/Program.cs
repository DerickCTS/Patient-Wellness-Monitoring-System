using Hangfire;
using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.Jobs;
using Microsoft.IdentityModel.Tokens; 
using Patient_Monitoring.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Patient_Monitoring.Services.Interfaces;
using Patient_Monitoring.Repositories.Implementations;
using Patient_Monitoring.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Patient_Monitoring.Models;



var builder = WebApplication.CreateBuilder(args);

const string AllowSpecificOrigins = "_allowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Entity Framework
builder.Services.AddDbContext<PatientMonitoringDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PatientMonitoringDbContext")
        ?? throw new InvalidOperationException("Connection string 'PatientMonitoringDbContext' not found.")));

// Register repositories and services
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IWellnessPlanService, WellnessPlanService>();
builder.Services.AddScoped<IWellnessPlanRepository, WellnessPlanRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IProgressService, ProgressService>();
builder.Services.AddScoped<IProgressRepository, ProgressRepository>();
builder.Services.AddScoped<IDiagnosisService, DiagnosisService>();
builder.Services.AddScoped<IDiagnosisRepository, DiagnosisRepository>();
builder.Services.AddScoped<SchedulingService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IPasswordHasher<Patient>, PasswordHasher<Patient>>();
builder.Services.AddScoped<IPasswordHasher<Doctor>, PasswordHasher<Doctor>>();


// Configure JWT Authentication
var keyBytes = Convert.FromBase64String(builder.Configuration["Jwt:Key"]
    ?? throw new ArgumentNullException("Jwt:Key is missing"));

//var keyBytes = Convert.FromBase64String("kGv/rT4iXb+ZcE7qFjJ8pL9sW0uYvN3xH6aV2dC5bO4=");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["AuthToken"];
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// Configure Hangfire
builder.Services.AddScoped<ScheduledNotificationJob>();
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("PatientMonitoringDbContext")));
builder.Services.AddHangfireServer();

var app = builder.Build();

DataSeeder.Seed(app);

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(AllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard();
app.MapControllers();

// Register recurring jobs
RecurringJob.AddOrUpdate<ScheduledNotificationJob>(
    "medicine-reminders", job => job.SendMedicineRemindersAsync(), Cron.Minutely);
RecurringJob.AddOrUpdate<ScheduledNotificationJob>(
    "appointment-reminders", job => job.SendAppointmentRemindersAsync(), Cron.Daily);
RecurringJob.AddOrUpdate<ScheduledNotificationJob>(
    "daily-wellness-reminders", job => job.SendDailyWellnessRemindersAsync(), Cron.Daily(8));

app.Run();

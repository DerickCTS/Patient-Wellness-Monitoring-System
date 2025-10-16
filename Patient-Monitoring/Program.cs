using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.Repository.Implementations;
using Patient_Monitoring.Repository.Interfaces;
using Patient_Monitoring.Services.Implementations;
using Patient_Monitoring.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<PatientMonitoringDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PatientMonitoringDbContext") ?? throw new InvalidOperationException("Connection string 'PatientMonitoringContext' not found.")));

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IPatientDoctorMappingRepository, PatientDoctorMappingRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();


// Register PatientDashboardService
builder.Services.AddScoped<IPatientDashboardService, PatientDashboardService>();

// Register PatientDashboardRepository
builder.Services.AddScoped<IPatientDashboardRepository, PatientDashboardRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "");

app.Run();

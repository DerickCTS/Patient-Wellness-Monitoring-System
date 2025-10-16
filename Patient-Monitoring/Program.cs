using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.Repositories.Implementations;
using Patient_Monitoring.Repositories.Interfaces;
using Patient_Monitoring.Services.Implementations;
using Patient_Monitoring.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<PatientMonitoringDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PatientMonitoringDbContext") ?? throw new InvalidOperationException("Connection string 'PatientMonitoringContext' not found.")));

// --- Repository Registration (Data Access Layer) ---
// Registers the repository interfaces with their concrete implementations.
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();


// --- Service Registration (Business Logic Layer) ---
// Registers the service interfaces with their concrete implementations.
// Note: Assuming 'SchedulingService' implements 'ISchedulingService' for better practice.
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<SchedulingService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

DataSeeder.Seed(app);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

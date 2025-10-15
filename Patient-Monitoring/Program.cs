using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.Repository.Implementation;
using Patient_Monitoring.Repository.Interface;
using Patient_Monitoring.Services.Implementation;
using Patient_Monitoring.Services.Interface;




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


// Add services to the container.
builder.Services.AddControllersWithViews();
// Optional: Add Swagger/OpenAPI support for API documentation (useful for testing)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PatientMonitoringDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PatientMonitoringDbContext") ?? throw new InvalidOperationException("Connection string 'PatientMonitoringContext' not found.")));

builder.Services.AddScoped<IDoctorService, DoctorService>();

builder.Services.AddScoped<IWellnessPlanService, WellnessPlanService>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IWellnessPlanRepository, WellnessPlanRepository>();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

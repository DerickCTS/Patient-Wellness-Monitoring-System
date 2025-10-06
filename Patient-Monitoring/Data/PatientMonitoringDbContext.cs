using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Models;

namespace Patient_Monitoring.Data
{
    public class PatientMonitoringDbContext : DbContext
    {
        public PatientMonitoringDbContext(DbContextOptions<PatientMonitoringDbContext> options) : base(options)
        {
        }

        public DbSet<AppointmentAlert> Appointment_Alerts { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<WellnessPlan> WellnessPlans { get; set; }
        public DbSet<Doctor> Doctor_Details { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<Patient> Patient_Details { get; set; }
        public DbSet<Diagnosis> Patient_Diagnoses { get; set; }
        public DbSet<Medication> Patient_Medications { get; set; }
        public DbSet<PatientDoctorMapper> Patient_Doctor_Mapper { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define relationships and constraints here if needed...
        }
        
    }
  
}



    


    
   





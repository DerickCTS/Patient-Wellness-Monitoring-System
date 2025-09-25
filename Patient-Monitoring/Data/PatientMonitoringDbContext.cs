using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Models;

namespace Patient_Monitoring.Data
{
    public class PatientMonitoringDbContext : DbContext
    {
        public PatientMonitoringDbContext(DbContextOptions<PatientMonitoringDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Define relationships and constraints here if needed...
        }
        public DbSet<Appointment_Alerts> Appointment_Alerts { get; set; }
        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<Custom_Wellness_Plans> Custom_Wellness_Plans { get; set; }
        public DbSet<Doctor_Details> Doctor_Details { get; set; }
        public DbSet<Disease> Disease { get; set; }
        public DbSet<Patient_Details> Patient_Details { get; set; }
        public DbSet<Patient_Diagnosis> Patient_Diagnosis { get; set; }
        public DbSet<Patient_Medication> Patient_Medication { get; set; }
        public DbSet<Patient_Doctor_Mapper> Patient_Doctor_Mapper { get; set; }
        public DbSet<General_Wellness_Plans> General_Wellness_Plans { get; set; }
    }
}





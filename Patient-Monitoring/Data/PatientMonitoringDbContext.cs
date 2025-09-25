using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Models;

namespace Patient_Monitoring.Data
{
    public class PatientMonitoringDbContext : DbContext
    {
        public PatientMonitoringDbContext(DbContextOptions<PatientMonitoringDbContext> options) : base(options)
        {
        }

        public DbSet<Appointment_Alert> Appointment_Alerts { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Wellness_Plan> Wellness_Plans { get; set; }
        public DbSet<Doctor_Detail> Doctor_Details { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<Patient_Detail> Patient_Details { get; set; }
        public DbSet<Patient_Diagnosis> Patient_Diagnoses { get; set; }
        public DbSet<Patient_Medication> Patient_Medications { get; set; }
        public DbSet<Patient_Doctor_Mapper> Patient_Doctor_Mapper { get; set; }
        public DbSet<Patient_Plan_Mapper> Patient_Plan_Mapper { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define relationships and constraints here if needed...
        }
        public DbSet<Appointment_Alerts> Appointment_Alerts { get; set; }
        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<Wellness_Plans> Wellness_Plans { get; set; }
        public DbSet<Doctor_Details> Doctor_Details { get; set; }
        public DbSet<Disease> Disease { get; set; }
        public DbSet<Patient_Details> Patient_Details { get; set; }
        public DbSet<Patient_Diagnosis> Patient_Diagnosis { get; set; }
        public DbSet<Patient_Medication> Patient_Medication { get; set; }
        public DbSet<Patient_Doctor_Mapper> Patient_Doctor_Mapper { get; set; }
        public DbSet<Patient_Plan_Mapper>  Patient_Plan_Mapper { get; set; }
    }
  
}



    


    
   





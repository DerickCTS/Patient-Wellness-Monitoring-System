using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Models;

namespace Patient_Monitoring.Data
{
    public class PatientMonitoringDbContext : DbContext
    {
        public PatientMonitoringDbContext(DbContextOptions<PatientMonitoringDbContext> options) : base(options)
        {
        }

        public DbSet<AppointmentAlert> AppointmentAlerts { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<WellnessPlan> WellnessPlans { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Diagnosis> Diagnoses { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<PatientDoctorMapper> PatientDoctorMapper { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<AssignmentPlanDetail> AssignmentPlanDetails { get; set; }
        public DbSet<DailyTaskLog> DailyTaskLogs { get; set; }
        public DbSet<WellnessPlanDetail> WellnessPlanDetails { get; set; }
        public DbSet<PatientPlanAssignment> PatientPlanAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define relationships and constraints here if needed...
        }
        
    }
  
}



    


    
   





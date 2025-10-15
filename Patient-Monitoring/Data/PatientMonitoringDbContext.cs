using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Models;

namespace Patient_Monitoring.Data
{
    public class PatientMonitoringDbContext : DbContext
    {
        public PatientMonitoringDbContext(DbContextOptions<PatientMonitoringDbContext> options) : base(options)
        {
        }


        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<WellnessPlan> WellnessPlans { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Diagnosis> Diagnoses { get; set; }
        public DbSet<MedicationSchedule> MedicationSchedules { get; set; }
        public DbSet<PatientDoctorMapper> PatientDoctorMapper { get; set; }
        public DbSet<PatientPlanAssignment> PatientPlanAssignments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<WellnessPlanDetail> WellnessPlanDetails { get; set; }
        public DbSet<AssignmentPlanDetail> AssignmentPlanDetails { get; set; }
        public DbSet<TaskLog> TaskLogs { get; set; }
        public DbSet<AppointmentSlot> AppointmentSlots { get; set; }
        public DbSet<DoctorAvailability> DoctorAvailabilities { get; set; }
        public DbSet<DoctorTimeOff> DoctorTimeOffs { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Notification> Notifications { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<PatientPlanAssignment>()
.HasOne(ppa => ppa.AssigningDoctor)
.WithMany(d => d.PatientPlanAssignments) // Assuming Doctor has a collection named PatientPlanAssignments
.HasForeignKey(ppa => ppa.AssignedByDoctorId)
.OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Diagnosis>()
                .HasOne(d => d.Patient)
                .WithMany(p => p.Diagnoses)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prescription>()
                .HasOne(p => p.Doctor)
                .WithMany(d => d.PrescribedMedications)
                .HasForeignKey(p => p.PrescribingDoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prescription>()
                .HasOne(p => p.Patient)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(p => p.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }

}













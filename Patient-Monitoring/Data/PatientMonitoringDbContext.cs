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
            // ====================================================================
            // CRITICAL FIX: Explicitly set table names to match SSMS PLURAL names.
            // This prevents EF Core from looking for tables that don't exist.
            // ====================================================================

            // The table is MISSING, so we'll use the desired singular name for creation
            modelBuilder.Entity<DoctorTimeOff>().ToTable("DoctorTimeOff");

            // Match the existing plural names in SSMS screenshots:
            modelBuilder.Entity<DoctorAvailability>().ToTable("DoctorAvailabilities"); // Matches SSMS
            modelBuilder.Entity<AppointmentSlot>().ToTable("AppointmentSlots");     // Matches SSMS
            modelBuilder.Entity<Appointment>().ToTable("Appointments");             // Matches SSMS
            modelBuilder.Entity<Doctor>().ToTable("Doctor_Details");         // Matches SSMS
            modelBuilder.Entity<Patient>().ToTable("Patient_Details");       // Matches SSMS


            // --------------------------------------------------------------------------------
            // 1. Configure the One-to-One Relationship between Appointment and AppointmentSlot
            // --------------------------------------------------------------------------------
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.AppointmentSlot)
                .WithOne(s => s.Appointment)
                .HasForeignKey<Appointment>(a => a.SlotID)
                .IsRequired(false);


            // --------------------------------------------------------------------------------
            // 2. Configure the Doctor relationships (One-to-Many)
            // --------------------------------------------------------------------------------

            // Doctor to DoctorAvailability
            modelBuilder.Entity<DoctorAvailability>()
                .HasOne(da => da.Doctor)
                .WithMany(d => d.DoctorAvailabilities)
                .HasForeignKey(da => da.DoctorID);

            // Doctor to DoctorTimeOff
            modelBuilder.Entity<DoctorTimeOff>()
                .HasOne(dto => dto.Doctor)
                .WithMany(d => d.DoctorTimeOffs)
                .HasForeignKey(dto => dto.DoctorID);

            // Doctor to AppointmentSlot
            modelBuilder.Entity<AppointmentSlot>()
                .HasOne(s => s.Doctor)
                .WithMany()
                .HasForeignKey(s => s.DoctorID);

            // Call the base method last
            base.OnModelCreating(modelBuilder);
        }
    }
}
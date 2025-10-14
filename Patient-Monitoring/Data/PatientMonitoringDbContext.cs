using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Models;

namespace Patient_Monitoring.Data
{
    public class PatientMonitoringDbContext : DbContext
    {
        public PatientMonitoringDbContext(DbContextOptions<PatientMonitoringDbContext> options) : base(options)
        {
        }

        // DbSets (No changes needed here, EF Core maps the models)
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
        public DbSet<DoctorAvailability> DoctorAvailabilities { get; set; }
        public DbSet<DoctorTimeOff> DoctorTimeOffs { get; set; }
        public DbSet<AppointmentSlot> AppointmentSlots { get; set; }


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
            modelBuilder.Entity<Doctor_Detail>().ToTable("Doctor_Details");         // Matches SSMS
            modelBuilder.Entity<Patient_Detail>().ToTable("Patient_Details");       // Matches SSMS


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
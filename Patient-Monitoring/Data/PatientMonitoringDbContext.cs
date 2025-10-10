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
        public DbSet<DoctorAvailability> DoctorAvailabilities { get; set; }
        public DbSet<DoctorTimeOff> DoctorTimesOff { get; set; }
        public DbSet<AppointmentSlot> AppointmentSlots { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define relationships and constraints here if needed...
            // --------------------------------------------------------------------------------
            // 1. Configure the One-to-One Relationship between Appointment and AppointmentSlot
            //    An Appointment is linked to one Slot, and that Slot can only be linked to one Appointment.
            // --------------------------------------------------------------------------------
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.AppointmentSlot)       // Appointment has one AppointmentSlot
                .WithOne(s => s.Appointment)          // AppointmentSlot has one Appointment
                .HasForeignKey<Appointment>(a => a.SlotID) // The foreign key is on the Appointment table
                .IsRequired(false);                   // SlotID is nullable (optional relationship)


            // --------------------------------------------------------------------------------
            // 2. Configure the Doctor relationships 
            //    (Assumes your existing Doctor model is named 'Doctor' or 'Doctor_Detail')
            // --------------------------------------------------------------------------------

            // Doctor to DoctorAvailability
            modelBuilder.Entity<DoctorAvailability>()
                .HasOne(da => da.Doctor)
                .WithMany() // Or specify a navigation property in Doctor if you have one
                .HasForeignKey(da => da.DoctorID);

            // Doctor to DoctorTimeOff
            modelBuilder.Entity<DoctorTimeOff>()
                .HasOne(dto => dto.Doctor)
                .WithMany() // Or specify a navigation property in Doctor if you have one
                .HasForeignKey(dto => dto.DoctorID);

            // Doctor to AppointmentSlot
            modelBuilder.Entity<AppointmentSlot>()
                .HasOne(s => s.Doctor)
                .WithMany() // Or specify a navigation property in Doctor if you have one
                .HasForeignKey(s => s.DoctorID);

            // Call the base method last
            base.OnModelCreating(modelBuilder);
        }
    }
  
}





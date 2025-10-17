
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Patient_Monitoring.Models
{
    public class Prescription
    {
        [Key]
        [Display(Name = "Prescription ID")]
        public int PrescriptionId { get; set; }

        [Required]
        [Display(Name = "Patient ID")]
        public required int PatientId { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Medication Name")]
        public required string MedicationName { get; set; } // e.g., "Paracetamol"

        [Required]
        [MaxLength(50)]
        public required string Dosage { get; set; } // e.g., "500mg"

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } // Medication start date

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; } // Medication end date

        [Required]
        [Display(Name = "Prescribing Doctor ID")]
        public required int PrescribingDoctorId { get; set; }

        [MaxLength(255)]
        public string? Instructions { get; set; } // Optional notes (e.g., "Take with food")


        [Required]
        [Display(Name = "Appointment Id")]
        public required int AppointmentId { get; set; }

        // Navigation Properties        
        public Patient Patient { get; set; } = null!;
        public Appointment Appointment { get; set; } = null!;

        [ForeignKey(nameof(PrescribingDoctorId))]
        public Doctor Doctor { get; set; } = null!;

        public ICollection<MedicationSchedule> MedicationSchedules { get; set; } = null!;
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Patient_Monitoring.Models;

namespace Patient_Monitoring.Models
{
    public class Prescription
    {
        [Key]
        public string PrescriptionId { get; set; } = null!;

        [Required]
        public string PatientId { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public required string MedicationName { get; set; } // e.g., "Paracetamol"

        [Required]
        [MaxLength(50)]
        public required string Dosage { get; set; } // e.g., "500mg"

        [Required]
        public DateTime StartDate { get; set; } // Medication start date

        [Required]
        public DateTime EndDate { get; set; } // Medication end date

        [Required]
        public string PrescribingDoctorId { get; set; } = null!;

        [MaxLength(255)]
        public string? Instructions { get; set; } // Optional notes (e.g., "Take with food")

        // Navigation Properties
        //[ForeignKey(nameof(PatientId))]
        public required Patient Patient { get; set; }

        //[ForeignKey(nameof(PrescribingDoctorId))]
        public required Doctor Doctor { get; set; }
    }
}
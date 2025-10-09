using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Patient_Monitoring.Models
{
    public class MedicationSchedule
    {
        [Key]
        public string ScheduleId { get; set; } = null!; // Primary Key

        [Required]
        public required string PrescriptionId { get; set; } // FK to Prescriptions table

        [Required]
        [MaxLength(20)]
        public required string TimeOfDay { get; set; } // e.g., "Forenoon", "Noon", "Evening"

        [Required]
        [Range(0.1, 10.0)]
        public float Quantity { get; set; } // Units to take (e.g., 1, 0.5)

        // Navigation Property
        //[ForeignKey(nameof(PrescriptionId))]
        public required Prescription Prescription { get; set; }
    }
}
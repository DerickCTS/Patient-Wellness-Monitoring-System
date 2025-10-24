using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace Patient_Monitoring.Models
{
    public class MedicationSchedule
    {
        [Key]
        [Display(Name = "Schedule ID")]
        [Required]
        public int ScheduleId { get; set; }

        [Required]
        [Display(Name = "Prescription ID")]
        public required int PrescriptionId { get; set; } // FK to Prescriptions table

        [Required]
        [MaxLength(20)]
        [Display(Name = "Time of Day")]
        public required string TimeOfDay { get; set; } // e.g., "Forenoon", "Noon", "Evening"

        [Required]
        [Range(0.1, 10.0)]
        public float Quantity { get; set; } // Units to take (e.g., 1, 0.5)

        // Navigation Property
        public Prescription Prescription { get; set; } = null!;
    }
}

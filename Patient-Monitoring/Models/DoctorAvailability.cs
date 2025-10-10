using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Patient_Monitoring.Models
{
    public class DoctorAvailability
    {
        [Key]
        [Required]
        public int AvailabilityID { get; set; } // Primary Key

        [Required]
        [StringLength(450)]
        public required string DoctorID { get; set; } // Foreign Key to Doctor

        [Required]
        [StringLength(10, ErrorMessage = "Day of week cannot exceed 10 characters.")]
        public required string DayOfWeek { get; set; } // e.g., "Monday"

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Start Time")]
        public required TimeSpan StartTime { get; set; } // Use TimeSpan for time of day

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "End Time")]
        public required TimeSpan EndTime { get; set; }

        [Required]
        [Display(Name = "Is Recurring")]
        public bool IsRecurring { get; set; } = true; // Default to true

        // Navigation property for Foreign Key relationship
        [ForeignKey("DoctorID")]
        public Doctor_Detail? Doctor { get; set; }
    }
}
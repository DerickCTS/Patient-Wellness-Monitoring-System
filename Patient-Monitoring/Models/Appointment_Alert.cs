using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.Models
{
    public class Appointment_Alert
    {
        [Key]
        [Required]
        [StringLength(50, ErrorMessage = "AlertID cannot exceed 50 characters.")]
        public required string AlertID { get; set; } // Primary Key

        [Required]
        [StringLength(50, ErrorMessage = "AppointmentID cannot exceed 50 characters.")]
        public required string AppointmentID { get; set; } // Foreign Key to Appointments
    }
}



using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.Models
{
    public class AppointmentAlert
    {
        [Key]
        [Required]
        
        public required string AlertID { get; set; } // Primary Key

        [Required]
        
        public required string AppointmentID { get; set; } // Foreign Key to Appointments

        public Appointment Appointment { get; set; } = null!;
    }
}



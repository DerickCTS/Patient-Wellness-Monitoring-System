using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.Models
{
    public class Appointments
    {
        [Required] public required string AppointmentID { get; set; } // Primary Key
        [Required] public required string PatientID { get; set; } // Foreign Key to Patient_Details
        [Required] public required string DoctorID { get; set; } // Foreign Key to Doctor_Details
        [Required] public required DateTime Appointment_Date_Time { get; set; }
        [Required] public required string Reason { get; set; }
        public string? Notes { get; set; } // Nullable for optional notes
    }
}

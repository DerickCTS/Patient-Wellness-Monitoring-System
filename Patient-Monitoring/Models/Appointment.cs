using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.Models
{
    public class Appointment
    {
        [Key]
        [Required]
        [StringLength(50, ErrorMessage = "AppointmentID cannot exceed 50 characters.")]
        public required string AppointmentID { get; set; } // Primary Key

        public Appointment_Alert? Alert { get; set; }

        public ICollection<Patient_Diagnosis> Diagnosis { get; set; } = null!;

        [Required]
        [StringLength(50, ErrorMessage = "PatientID cannot exceed 50 characters.")]
        public required string PatientID { get; set; } // Foreign Key to Patient_Details

        public Patient_Detail Patient { get; set; } = null!;

        [Required]
        [StringLength(50, ErrorMessage = "DoctorID cannot exceed 50 characters.")]
        public required string DoctorID { get; set; } // Foreign Key to Doctor_Details

        public Doctor_Detail Doctor { get; set; } = null!;

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Appointment Date & Time")]
        public DateTime Appointment_Date_Time { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Reason cannot exceed 200 characters.")]
        public required string Reason { get; set; }

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string? Notes { get; set; } // Nullable for optional notes
    }
}

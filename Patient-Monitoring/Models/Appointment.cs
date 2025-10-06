using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.Models
{
    public class Appointment
    {
        [Key]
        [Required]
        [StringLength(50, ErrorMessage = "AppointmentID cannot exceed 50 characters.")]
        [DisplayName("Appointment Id")]
        public required string AppointmentID { get; set; } // Primary Key


        [Required]
        [StringLength(50, ErrorMessage = "PatientID cannot exceed 50 characters.")]
        [DisplayName("Patient Id")]
        public required string PatientID { get; set; } // Foreign Key to Patient_Details


        [Required]
        [StringLength(50, ErrorMessage = "DoctorID cannot exceed 50 characters.")]
        [DisplayName("Doctor Id")]
        public required string DoctorID { get; set; } // Foreign Key to Doctor_Details


        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Appointment Date & Time")]
        public DateTime Appointment_Date_Time { get; set; }


        [Required]
        [StringLength(200, ErrorMessage = "Reason cannot exceed 200 characters.")]
        public required string Reason { get; set; }


        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string? Notes { get; set; } // Nullable for optional notes


        public AppointmentAlert? Alert { get; set; }
        public Patient Patient { get; set; } = null!;
        public Doctor Doctor { get; set; } = null!;
        public ICollection<Diagnosis> Diagnoses { get; set; } = null!;
    }
}

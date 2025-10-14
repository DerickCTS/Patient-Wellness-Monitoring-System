using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Patient_Monitoring.Models
{
    public class Appointment
    {
        [Key]
        [Required]
        [StringLength(50, ErrorMessage = "AppointmentID cannot exceed 50 characters.")]
        public required string AppointmentID { get; set; } // Primary Key

        [Required]
        [StringLength(50, ErrorMessage = "PatientID cannot exceed 50 characters.")]
        public required string PatientID { get; set; } // Foreign Key to Patient

        [Required]
        [StringLength(50, ErrorMessage = "DoctorID cannot exceed 50 characters.")]
        public required string DoctorID { get; set; } // Foreign Key to Doctor

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Appointment Date & Time")]
        public DateTime Appointment_Date_Time { get; set; }

        // --- MODIFICATIONS START HERE ---

        // 1. Increased length for detailed patient issue
        [Required]
        [StringLength(1000, ErrorMessage = "Reason cannot exceed 1000 characters.")]
        public required string Reason { get; set; }

        public DateTime AppointmentDate { get; set; }
        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string? Notes { get; set; } // Nullable for optional notes

        // 2. Foreign Key to the booked slot
        public int? SlotID { get; set; } // Nullable if the appointment wasn't booked via a slot system

        // 3. Status for approval workflow: 'Pending Approval', 'Confirmed', 'Rejected', etc.
        [Required]
        [StringLength(50)]
        public required string Status { get; set; } = "Pending Approval";

        // 4. Reason provided by doctor if rejected
        public string? RejectionReason { get; set; }

        // --- NAVIGATION PROPERTIES ---

        [ForeignKey("SlotID")]
        public AppointmentSlot? AppointmentSlot { get; set; }

        public required DateTime RequestedOn { get; set; } 
        // Assuming you have Doctor and Patient models for these:
        // [ForeignKey("DoctorID")]
        // public Doctor? Doctor { get; set; }
        // [ForeignKey("PatientID")]
        // public Patient? Patient { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Patient_Monitoring.Models
{
    public class Appointment
    {
        [Key]
        [Required]
        [Display(Name = "Appointment ID")]
        public int AppointmentId { get; set; }


        [Required]
        [Display(Name = "Patient ID")]
        public required int PatientId { get; set; }


        [Required]
        [Display(Name = "Doctor ID")]
        public required int DoctorId { get; set; }


        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Appointment Date & Time")]
        public DateTime AppointmentDate { get; set; }

        
        [Required]
        [StringLength(1000, ErrorMessage = "Reason cannot exceed 1000 characters.")]
        public required string Reason { get; set; }


        //[StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        //public string? Notes { get; set; } // Nullable for optional notes

        [Required]
        [Display(Name = "Slot ID")]
        public int? SlotId { get; set; }


        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Requested On")]
        public required DateTime RequestedOn { get; set; }


        [Required]
        [StringLength(100)]
        public required string Status { get; set; } = "Pending Approval";
        

        [StringLength(1000, ErrorMessage = "Rejection reason cannot exceed 1000 characters.")]
        [Display(Name = "Rejection Reason")]
        public string? RejectionReason { get; set; }


        [ForeignKey("SlotId")]
        public AppointmentSlot? AppointmentSlot { get; set; }
        public ICollection<Prescription>? Prescriptions { get; set; }
        public ICollection<Diagnosis> Diagnoses { get; set; } = null!;
        public Patient Patient { get; set; } = null!;
        public Doctor Doctor { get; set; } = null!;
    }
}
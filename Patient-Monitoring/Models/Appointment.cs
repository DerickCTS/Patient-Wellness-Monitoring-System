using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Patient_Monitoring.Models
{
    public class Appointment
    {
        [Key]
        [Required]
        [Display(Name = "Appointment ID")]
        public required string AppointmentId { get; set; }


        [Required]
        [Display(Name = "Patient ID")]
        public required string PatientId { get; set; }


        [Required]
        [Display(Name = "Doctor ID")]
        public required string DoctorId { get; set; }


        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Appointment Date & Time")]
        public DateTime AppointmentDate { get; set; }


        [Required]
        [StringLength(1000, ErrorMessage = "Reason cannot exceed 1000 characters.")]
        public required string Reason { get; set; }


        [Required]
        [Display(Name = "Slot ID")]
        public int SlotId { get; set; }


        // 3. Status for approval workflow: 'Pending Approval', 'Confirmed', 'Rejected', etc.
        [Required]
        [StringLength(100)]
        public string Status { get; set; } = "Pending Approval";


        [StringLength(1000, ErrorMessage = "Rejection reason cannot exceed 1000 characters.")]
        [Display(Name = "Rejection Reason")]
        public string? RejectionReason { get; set; }

        [ForeignKey("SlotId")]
        public AppointmentSlot? AppointmentSlot { get; set; }
        public ICollection<Diagnosis> Diagnoses { get; set; } = null!;
        public Patient Patient { get; set; } = null!;
        public Doctor Doctor { get; set; } = null!;
    }
}
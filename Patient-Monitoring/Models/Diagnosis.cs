using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.Models
{
    public class Diagnosis
    {
        [Required(ErrorMessage = "Diagnosis ID is required.")]
        [StringLength(10, ErrorMessage = "Diagnosis ID cannot exceed 10 characters.")]
        [Display(Name = "Diagnosis ID")]
        [Key]
        public required string DiagnosisID { get; set; }


        [Required(ErrorMessage = "Patient ID is required.")]
        [StringLength(10, ErrorMessage = "Patient ID cannot exceed 10 characters.")]
        [Display(Name = "Patient ID")]
        public required string PatientID { get; set; }


        [Required(ErrorMessage = "Disease ID is required.")]
        [StringLength(10, ErrorMessage = "Disease ID cannot exceed 10 characters.")]
        [Display(Name = "Disease ID")]
        public required string DiseaseId { get; set; }


        [Required(ErrorMessage = "Appointment ID is required.")]
        [StringLength(10, ErrorMessage = "Appointment ID cannot exceed 10 characters.")]
        [Display(Name = "Appointment ID")]
        public required string AppointmentId { get; set; }


        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        [Display(Name = "Diagnosis Description")]
        public required string? Description { get; set; }


        public Patient Patient { get; set; } = null!;
        public Disease Disease { get; set; } = null!;
        public Appointment Appointment { get; set; } = null!;
    }
}


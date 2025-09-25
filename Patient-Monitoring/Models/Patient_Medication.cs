using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.Models
{
    public class Patient_Medication
    {
        [Required(ErrorMessage = "Medication ID is required.")]
        [StringLength(10, ErrorMessage = "Medication ID cannot exceed 10 characters.")]
        [Display(Name = "Medication ID")]
        [Key]
        public required string MedicationID { get; set; } // Primary Key

        [Required(ErrorMessage = "Patient ID is required.")]
        [StringLength(10, ErrorMessage = "Patient ID cannot exceed 10 characters.")]
        [Display(Name = "Patient ID")]
        public required string PatientID { get; set; } // Foreign Key to Patient_Details

        [Required(ErrorMessage = "Medication Name is required.")]
        [StringLength(100, ErrorMessage = "Medication Name cannot exceed 100 characters.")]
        [Display(Name = "Medication Name")]
        public required string MedicationName { get; set; }

        [Required(ErrorMessage = "Dosage is required.")]
        [StringLength(50, ErrorMessage = "Dosage cannot exceed 50 characters.")]
        [Display(Name = "Dosage")]
        public required string Dosage { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public required DateTime Start_Date { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime? End_Date { get; set; } // Nullable for ongoing medications

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        [Display(Name = "Description")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "Prescribing Doctor ID is required.")]
        [StringLength(10, ErrorMessage = "Prescribing Doctor ID cannot exceed 10 characters.")]
        [Display(Name = "Prescribing Doctor ID")]
        public required string PrescribingDoctorId { get; set; } // Foreign Key to Doctor_Details
    }
}

using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.Models
{
    public class Patient_Doctor_Mapper
    {
        [Required]
        [StringLength(10, ErrorMessage = "Patient ID cannot exceed 10 characters.")]
        [Display(Name = "Patient ID")]
        [Key]
        public required string PatientID { get; set; } // Foreign Key to Patient_Details

        [Required]
        [StringLength(10, ErrorMessage = "Doctor ID cannot exceed 10 characters.")]
        [Display(Name = "Doctor ID")]
        public required string DoctorID { get; set; } // Foreign Key to Doctor_Details
    }
}

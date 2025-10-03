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

        public Patient_Detail Patient { get; set; } = null!;

        [Required]
        [StringLength(10, ErrorMessage = "Doctor ID cannot exceed 10 characters.")]
        [Display(Name = "Doctor ID")]
        public required string DoctorID { get; set; } // Foreign Key to Doctor_Details

        public Doctor_Detail Doctor_Detail { get; set; } = null!;
    }
}

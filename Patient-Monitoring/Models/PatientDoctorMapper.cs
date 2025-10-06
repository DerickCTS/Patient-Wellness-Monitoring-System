using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.Models
{
    public class PatientDoctorMapper
    {
        [Required]
        [StringLength(10, ErrorMessage = "Patient ID cannot exceed 10 characters.")]
        [Display(Name = "Patient ID")]
        [Key]
        public required string PatientID { get; set; } 


        [Required]
        [StringLength(10, ErrorMessage = "Doctor ID cannot exceed 10 characters.")]
        [Display(Name = "Doctor ID")]
        public required string DoctorID { get; set; } 


        public Doctor Doctor { get; set; } = null!;

        public Patient Patient { get; set; } = null!;
    }
}

using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.Models
{
    public class PatientDoctorMapper
    {
        [Required]
        [Display(Name = "Patient ID")]
        [Key]
        public required string PatientID { get; set; } 


        [Required]
        [Display(Name = "Doctor ID")]
        public required string DoctorID { get; set; } 


        public Doctor Doctor { get; set; } = null!;

        public Patient Patient { get; set; } = null!;
    }
}

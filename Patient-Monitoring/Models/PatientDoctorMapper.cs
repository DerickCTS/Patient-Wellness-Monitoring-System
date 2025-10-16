using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Patient_Monitoring.Models
{
    public class PatientDoctorMapper
    {
        [Required]
        [Display(Name = "Patient ID")]
        [Key]
        public required string PatientId { get; set; }


        [Required]
        [Display(Name = "Doctor ID")]
        public required string DoctorId { get; set; }

        [Required]
        [Display(Name = "Patient Since")]
        [DataType(DataType.Date)]
        public required DateTime AssignedDate { get; set; }

        public Doctor Doctor { get; set; } = null!;

        public Patient Patient { get; set; } = null!;
    }
}
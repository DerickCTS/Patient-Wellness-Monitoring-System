using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.Models
{
    public class Patient_Doctor_Mapper
    {
        [Required] public required string PatientID { get; set; } // Foreign Key to Patient_Details
        [Required] public required string DoctorID { get; set; } // Foreign Key to Doctor_Details
    }
}

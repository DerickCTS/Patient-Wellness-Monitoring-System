using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.Models
{
    public class Patient_Diagnosis
    {
        [Required] public required string DiagnosisID { get; set; } // Primary Key
        [Required] public required string PatientID { get; set; } // Foreign Key to Patient_Details
        [Required] public required string DiseaseId { get; set; }
        [Required] public required string AppointmentId { get; set; } // Foreign Key to Appointments
        [Required] public required string? Description { get; set; }
    }
}

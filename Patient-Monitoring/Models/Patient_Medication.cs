using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
namespace Patient_Monitoring.Models
{
    public class Patient_Medication
    {
        [Required] public required string MedicationID { get; set; } // Primary Key

        [Required] public required string PatientID { get; set; } // Foreign Key to Patient_Details
        [Required] public required string MedicationName { get; set; }
        [Required] public required string Dosage { get; set; }

        [Required] public required DateTime Start_Date { get; set; }
        public DateTime? End_Date { get; set; } // Nullable for ongoing medications
        [Required] public required string Description { get; set; }

        [Required] public required string PrescribingDoctorId { get; set; } //Foreign Key to Doctor_Details
    }
}

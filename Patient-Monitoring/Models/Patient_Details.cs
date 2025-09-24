using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
namespace Patient_Monitoring.Models
{
        public class Patient_Details
        {
        
            [Required] public required string PatientID { get; set; } // Primary Key

        // Personal Info
       
            [Required] public required string FirstName { get; set; }
       
        public string? LastName { get; set; }
            [Required] public required DateTime DateOfBirth { get; set; }
            [Required] public required string Gender { get; set; }

        // Contact Info
            [Required] public required string ContactNumber { get; set; }
            [Required] public required string Email { get; set; }
            [Required] public required string Address { get; set; }

        // Emergency Contact
            [Required] public required string EmergencyContact { get; set; }

/*            // Physical Metrics
            public decimal HeightCm { get; set; }
            public decimal WeightKg { get; set; }
            public decimal? BMI { get; set; } // Nullable

            public required string BloodType { get; set; }

            // Lifestyle & Medical Info
            public bool? HasChronicConditions { get; set; } 
            public required string ChronicConditionDetails { get; set; }*/

            // System Info
            public DateTime RegistrationDate { get; set; }
        }

    }

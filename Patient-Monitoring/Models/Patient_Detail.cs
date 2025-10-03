using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
namespace Patient_Monitoring.Models
{
    public class Patient_Detail
    {
        [Display(Name = "Patient ID")]
        [Key]
        [Required] public required string PatientID { get; set; } // Primary Key

        // Navigation Properties
        public ICollection<Patient_Diagnosis> PatientDiagnoses { get; set; } = new List<Patient_Diagnosis>();

        // Personal Info

        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        [Display(Name = "First Name")]

        [Required] public required string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        [Display(Name = "Last Name")]

        public string LastName { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]

        [Required] public required DateTime DateOfBirth { get; set; }
        [Required] public required string Gender { get; set; }

        // Contact Info

        [Required]
        [Phone(ErrorMessage = "Invalid phone number.")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Contact number must be between 10 and 15 digits.")]

        public required string ContactNumber { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Required] public required string Email { get; set; }
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        [Required] public required string Address { get; set; }

        // Emergency Contact

        [Phone(ErrorMessage = "Invalid emergency contact number.")]
        [Display(Name = "Emergency Contact")]

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
        [Display(Name = "Registration Date")]
        public DateTime RegistrationDate { get; set; }
    }

}

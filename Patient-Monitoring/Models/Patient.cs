using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Patient_Monitoring.Models;
namespace Patient_Monitoring.Models
{
    public class Patient
    {

        [Key]
        [Display(Name = "Patient ID")]
        public required string PatientId { get; set; } // Primary Key


        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        [Display(Name = "First Name")]
        [Required]
        public required string FirstName { get; set; }


        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        [Required]
        public required DateTime DateOfBirth { get; set; }


        [Required]
        public required string Gender { get; set; }


        [Required]
        [Phone(ErrorMessage = "Invalid phone number.")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Contact number must be between 10 and 15 digits.")]
        [DisplayName("Contact Number")]
        [DataType(DataType.PhoneNumber)]
        public required string ContactNumber { get; set; }


        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Required]
        public required string Email { get; set; }

        [Required]
        [Display(Name = "Blood Group")]
        public required string BloodGroup { get; set; } 


        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        [Required]
        public required string Address { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public required string EmergencyContactName { get; set; }


        [Phone(ErrorMessage = "Invalid emergency contact number.")]
        [Display(Name = "Emergency Contact Number")]
        [Required]
        public required string EmergencyContactNumber { get; set; }

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
        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; }


        [Display(Name = "Profile Image")]
        public string? ProfileImage { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }


        public ICollection<Prescription>? Prescriptions { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
        public PatientDoctorMapper PersonalizedDoctorMapper { get; set; } = null!;
        public ICollection<Appointment>? Appointments { get; set; }
        public ICollection<Diagnosis>? Diagnoses { get; set; }
        public ICollection<PatientPlanAssignment>? AssignedPlans { get; set; }

    }

}
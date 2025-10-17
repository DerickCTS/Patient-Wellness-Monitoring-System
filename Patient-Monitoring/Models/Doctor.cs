
using System.ComponentModel.DataAnnotations;
using System.Security.Permissions;

namespace Patient_Monitoring.Models
{
    public class Doctor
    {
        [Required]
        [Display(Name = "Doctor ID")]
        [Key]
        public int DoctorId { get; set; }


        [Required]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        [Display(Name = "First Name")]
        public required string FirstName { get; set; }


        [Required]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        [Display(Name = "Last Name")]
        public required string LastName { get; set; }


        [Required]
        [StringLength(50, ErrorMessage = "Specialization cannot exceed 50 characters.")]
        public required string Specialization { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "Education details cannot exceed 100 characters.")]
        public required string Education { get; set; }


        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Contact number must be between 10 and 15 digits.")]
        [Display(Name = "Contact Number")]
        [DataType(DataType.PhoneNumber)]
        [Required]
        public required string ContactNumber { get; set; }


        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [Required]
        public required string Email { get; set; }


        [Display(Name = "Profile Image URL")]
        public string? ProfileImage { get; set; }


        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Doctor Since")]
        public DateTime DoctorSince { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }


        public PatientDoctorMapper AssignedPatient { get; set; } = null!;

        public ICollection<Appointment>? Appointments { get; set; }

        public ICollection<Prescription>? PrescribedMedications { get; set; }

        public ICollection<WellnessPlan>? FormulatedWellnessPlans { get; set; }

        public ICollection<PatientPlanAssignment>? PatientPlanAssignments { get; set; }

        public ICollection<DoctorAvailability> DoctorAvailabilities { get; set; } = null!;

        public ICollection<DoctorTimeOff>? DoctorTimeOffs { get; set; } = null!;
    }
}
using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.DTOs
{
    public class PatientRegisterDTO
    {
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
        public required string ContactNumber { get; set; }
        
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Required] 
        public required string Email { get; set; }

        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        [Required] 
        public required string Address { get; set; }


        [Phone(ErrorMessage = "Invalid emergency contact number.")]
        [Display(Name = "Emergency Contact")]
        [Required]
        public required string EmergencyContact { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name  = "Confirm Password")]
        public required string ConfirmPassword { get; set; }
    }
}

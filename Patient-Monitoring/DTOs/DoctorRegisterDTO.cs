using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.DTOs
{
    public class DoctorRegisterDTO
    {
        // Personal Info
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
        [Required] 
        public required string ContactNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [Required] 
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string ConfirmPassword { get; set; }

    }
}

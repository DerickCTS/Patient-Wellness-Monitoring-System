using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.DTOs
{
    public class UserLoginDTO
    {
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Required] 
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        public string Role { get; set; }
    }
}

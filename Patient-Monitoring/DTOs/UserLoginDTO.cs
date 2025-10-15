using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.DTOs
{
    public class UserLoginDTO
    {
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Required] 
        public required string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public required string Password { get; set; }

        [Required]
        public string Role { get; set; } = null!;

        public string? ClientId { get; set; }
    }
}

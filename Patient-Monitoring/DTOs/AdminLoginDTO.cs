using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.DTOs
{
    public class AdminLoginDTO
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.DTOs
{
    public class UpdatePatientInfoDTO
    {
        [Required]
        [StringLength(50)]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public required string LastName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [Phone]
        public required string ContactNumber { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(10)]
        public required string Gender { get; set; }

        [Required]
        [StringLength(200)]
        public required string Address { get; set; }

        [Required]
        [StringLength(100)]
        public required string EmergencyContactName { get; set; }

        [Required]
        [Phone]
        public required string EmergencyContactNumber { get; set; }

        public string? ProfileImage { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System;

namespace Patient_Monitoring.DTOs.PatientDashboard
{
    public class UpdatePatientProfileDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [Length(10, 10, ErrorMessage = "Contact number must be 10 digits long.")]
        public string ContactNumber { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        // Updated field name to match the existing PatientProfileDto structure
        public string EmergencyContactName { get; set; } = string.Empty;

        [Required]
        [Phone]
        // New required field for the specific contact number
        public string EmergencyContactNumber { get; set; } = string.Empty;

        [Required]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
    }
}

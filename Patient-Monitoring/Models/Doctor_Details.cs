using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.Models
{
    public class Doctor_Details
    {
        [Required] public required string DoctorID { get; set; } // Primary Key
        // Personal Info
        [Required] public required string FirstName { get; set; }
        [Required] public required string LastName { get; set; }
        [Required] public required string Specialization { get; set; }
        [Required] public required string ContactNumber { get; set; }
        [Required] public required string Email { get; set; }
        [Required] public required string Password { get; set; }
    }
}

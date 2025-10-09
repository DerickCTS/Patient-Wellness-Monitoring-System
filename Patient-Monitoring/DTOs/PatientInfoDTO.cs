namespace Patient_Monitoring.DTOs
{
    public class PatientInfoDTO
    {
        public required string PatientId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public required string Gender { get; set; }
        public required string ContactNumber { get; set; }
        public required string Email { get; set; }
        public required string Address { get; set; }
        public required string EmergencyContactName { get; set; }
        public required string EmergencyContactNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string? ProfileImage { get; set; }
    }
}

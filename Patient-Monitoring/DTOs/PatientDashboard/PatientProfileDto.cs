namespace Patient_Monitoring.DTOs.PatientDashboard
{
    public class PatientProfileDto
    {
        public string FullName { get; set; }
        public int PatientId { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNumber { get; set; }
    }
}

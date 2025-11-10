namespace Patient_Monitoring.DTOs.DoctorDashboard
{
    public class DoctorProfileDto
    {
        public string DoctorName { get; set; }
        public string Specialty { get; set; }
        public string DoctorId { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime Joined { get; set; }
        public string Education { get; set; }
        public DateTime RegisteredSince { get; set; }
    }
}

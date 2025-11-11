namespace Patient_Monitoring.DTOs.DoctorDashboard
{
    public class DoctorProfileDto
    {
        public string firstName { get; set; }
        public string lastName { get; set; }

        public string Specialty { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public DateTime Joined { get; set; }
        public string Education { get; set; }
        public DateTime RegisteredSince { get; set; }
    }
}

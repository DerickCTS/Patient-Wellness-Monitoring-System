namespace Patient_Monitoring.DTOs.PatientDashboard
{
    public class DoctorDetailsDto
    {
        public string FullName { get; set; }
        public string Specialization { get; set; }
        public int DoctorId { get; set; }
        public string Qualification { get; set; } // MD from Johns Hopkins, etc.
        public DateTime PatientSince { get; set; } // from PatientDoctorMapper
        public int TotalVisits { get; set; }
        public DateTime LastVisit { get; set; }
        public string Languages { get; set; }
        public string LatestNote { get; set; } // Placeholder for doctor's latest comment/progress
    }
}

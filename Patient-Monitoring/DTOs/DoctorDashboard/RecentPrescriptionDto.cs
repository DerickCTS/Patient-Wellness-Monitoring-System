namespace Patient_Monitoring.DTOs.DoctorDashboard
{
    public class RecentPrescriptionDto
    {
        public string MedicationName { get; set; }
        public string PatientName { get; set; }
        public string Dosage { get; set; }
        public int Frequency { get; set; }
        public string DateRange { get; set; }
        public string Status { get; set; }
    }
}

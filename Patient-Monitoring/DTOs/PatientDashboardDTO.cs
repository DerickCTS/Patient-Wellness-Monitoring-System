namespace Patient_Monitoring.DTOs
{
    public class PatientDashboardDTO
    {
        public required PatientInfoDTO PatientInfo { get; set; }
        public required AssignedDoctorDTO AssignedDoctor { get; set; }
        public required List<PrescriptionDTO> ActivePrescriptions { get; set; }
        public required List<DailyTaskLogDTO> RecentTaskLogs { get; set; }
    }
}

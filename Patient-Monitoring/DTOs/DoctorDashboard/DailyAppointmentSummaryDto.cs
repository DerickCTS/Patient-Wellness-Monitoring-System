namespace Patient_Monitoring.DTOs.DoctorDashboard
{
    public class DailyAppointmentSummaryDto
    {
        public int Completed { get; set; }
        public int Remaining { get; set; }
        public int Total { get; set; }
    }
}

namespace Patient_Monitoring.DTOs.DoctorDashboard
{
    public class DoctorDashboardDto
    {
        public DashboardStatsDto Stats { get; set; } = null!;
        public List<WeeklyAppointmentSummaryItem> WeeklyAppointmentSummary { get; set; } = null!;
        public List<WeeklyScheduleItem> WeeklyScheduleSummary { get; set; } = null!;
        public List<TodaysAppointmentSummaryItem> TodaysAppointment { get; set; } = null!;
        public List<RecentPrescriptionSummaryItem> RecentPrescription { get; set; } = null!;
    }

    public class DashboardStatsDto
    {
        public int TotalPatients { get; set; }
        public int TodaysAppointments { get; set; }
        public int ActivePrescriptions { get; set; }
        public int TotalWellnessPlans { get; set; }
        public int CompletedToday { get; set; }
        //public int RemainingToday { get; set; }
    }

    public class WeeklyAppointmentSummaryItem
    {
        public string Day { get; set; } // Mon, Tue, etc.
        public int Approved { get; set; }
        public int Rejected { get; set; }
    }

    public class WeeklyScheduleItem
    {
        public string Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }

    public class TodaysAppointmentSummaryItem
    {
        public string ProfileUrl { get; set; } = null!;
        public string PatientName { get; set; } = null!;
        public int PatientAge { get; set; }
        public TimeSpan AppointmentStartTime { get; set; }
        public string Reason { get; set; } = null!;
        public string Status => DateTime.Today.Add(AppointmentStartTime).AddMinutes(20) < DateTime.Now ? "Completed" : "Scheduled";
    }

    public class RecentPrescriptionSummaryItem
    {
        public string MedicationName { get; set; } = null!;
        public string PatientName { get; set; } = null!;
        public string Dosage { get; set; } = null!;
        public int Frequency { get; set; } 
        public string MedicationStartDate { get; set; } = null!;
        public string MedicationEndDate { get; set; } = null!;
    }
}

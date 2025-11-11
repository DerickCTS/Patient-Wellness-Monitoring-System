namespace Patient_Monitoring.DTOs.PatientDashboard
{
    public class PatientDashboardDto
    {
        // 1. Patient Profile Info (from Patients table)
        public PatientProfileDto PatientProfile { get; set; }

        // 2. Personal Doctor & Care Relationship (from Doctors & PatientDoctorMapper)
        public DoctorDetailsDto PersonalDoctor { get; set; }

        // 3. Upcoming Appointments (from Appointments table)
        public List<AppointmentSummaryDto> UpcomingAppointments { get; set; }

        // 4. Active Prescriptions (from Prescription & MedicationSchedules)
        public List<PrescriptionSummaryDto> ActivePrescriptions { get; set; }

        // 5. Recent Activity / Task Logs (from TaskLog)
        public List<TaskLogSummaryDto> RecentActivities { get; set; }

    }
}

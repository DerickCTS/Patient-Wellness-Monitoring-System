using System;

namespace Patient_Monitoring.DTOs.PatientDashboard
{
    // DTO for the "Upcoming Appointments" list widget
    public class AppointmentSummaryDto
    {
        public int AppointmentId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public DateTime AppointmentDateTime { get; set; }
        public string Status { get; set; } = string.Empty; // e.g., "confirmed", "pending"
        public string Description { get; set; } = string.Empty;
    }
}

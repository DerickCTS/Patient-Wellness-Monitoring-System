namespace Patient_Monitoring.DTOs.Appointment
{
    public class UpcomingAppointmentDto
    {
        public string PatientName { get; set; }
        public int PatientId { get; set; }
        public string PatientAge { get; set; }
        public string PatientGender { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string AppointmentReason { get; set; }
    }
}

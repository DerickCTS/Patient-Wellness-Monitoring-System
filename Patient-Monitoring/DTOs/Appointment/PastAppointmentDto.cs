namespace Patient_Monitoring.DTOs.Appointment
{
    public class PastAppointmentDto
    {
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string DoctorName { get; set; }
        public string Specialization { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
    }
}
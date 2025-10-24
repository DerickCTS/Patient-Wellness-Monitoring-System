namespace Patient_Monitoring.DTOs.Appointment
{
    public class DoctorSlotViewDto
    {
        public TimeSpan Time { get; set; }
        // public string DisplayTime => DateTime.Today.Add(Time).ToString("hh:mm tt");

        public string DisplayTime { get; set; }
        public string Status { get; set; } // "Available", "Booked", "Break"
        public string PatientName { get; set; } // Null if Available
        public int AppointmentId { get; set; } // Null if Available
    }
}
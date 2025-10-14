namespace Patient_Monitoring.DTOs
{
    public class DoctorSlotViewDto
    {
        public TimeSpan Time { get; set; }
        public string DisplayTime => DateTime.Today.Add(Time).ToString("hh:mm tt");
        public string Status { get; set; } // "Available", "Booked", "Break"
        public string PatientName { get; set; } // Null if Available
        public string AppointmentID { get; set; } // Null if Available
    }
}
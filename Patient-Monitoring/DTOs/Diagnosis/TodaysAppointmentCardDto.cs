public class TodaysAppointmentCardDto
{
    public string AppointmentId { get; set; }
    public string PatientName { get; set; }
    public string Gender { get; set; }
    public int Age { get; set; }
    public string AppointmentTime { get; set; } // e.g., "09:00 AM - 09:30 AM"
    public string ContactNumber { get; set; }
    public string PatientId { get; set; }
    public string ChiefComplaint { get; set; } // The 'Reason' for the appointment
}
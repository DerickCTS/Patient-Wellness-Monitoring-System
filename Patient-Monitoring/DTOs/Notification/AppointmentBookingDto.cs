namespace Patient_Monitoring.DTOs.Notification
{
    public class AppointmentBookingDto
    {
        public required string PatientId { get; set; }
        public required string DoctorId { get; set; }
        public required int SlotId { get; set; }
        public required string Reason { get; set; }

    }
}

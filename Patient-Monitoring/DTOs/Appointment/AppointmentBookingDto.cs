namespace Patient_Monitoring.DTOs
{
    public class AppointmentBookingDto
    {
        public required int PatientId { get; set; }
        public required int DoctorId { get; set; }
        public required int SlotId { get; set; }
        public required string Reason { get; set; }

    }
}

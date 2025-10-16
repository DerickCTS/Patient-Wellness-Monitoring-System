namespace Patient_Monitoring.DTOs
{
    public class AppointmentBookingDto
    {
        public required string PatientID { get; set; }
        public required string DoctorID { get; set; }
        public required int SlotID { get; set; }
        public required string Reason { get; set; }

    }
}

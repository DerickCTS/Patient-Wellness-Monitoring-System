namespace Patient_Monitoring.DTOs.Appointment
{
    public class AppointmentStatusUpdateDto
    {
        public required int AppointmentId { get; set; }
        //Must be "Approve" or "Reject"
        public required string Action { get; set; }
        public string? RejectionReason { get; set; }
    }
}

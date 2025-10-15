namespace Patient_Monitoring.DTOs.Notification
{
    public class AppointmentStatusUpdateDto
    {
        public required string AppointmentId { get; set; }
        //Must be "Approve" or "Reject"
        public required string Action { get; set; }
        public string? RejectionReason { get; set; }
    }
}

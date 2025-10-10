namespace Patient_Monitoring.DTOs
{
    public class AppointmentStatusUpdateDto
    {
        public required string AppointmentID { get; set; }
        //Must be "Approve" or "Reject"
        public required string Action { get; set; }
        public string? RejectionReason { get; set; }
    }
}

namespace Patient_Monitoring.DTOs
{
    public class DailyTaskLogDTO
    {
        public required string LogId { get; set; }
        public required string AssignmentId { get; set; }
        public DateTime TaskDate { get; set; }
        public required string Status { get; set; } // completed, pending, missed
        public required DateTime CompletedAt { get; set; }

        public string? PatientNotes { get; set; }
    }
}

using System;

namespace Patient_Monitoring.DTOs.PatientDashboard
{
    public class TaskLogSummaryDto
    {
        public int TaskLogId { get; set; }
        public DateTime DateCompleted { get; set; }
        public string TaskName {  get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;// e.g., "30-minute walk completed"

    }
}

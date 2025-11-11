using System;

namespace Patient_Monitoring.DTOs.PatientDashboard
{
    public class TaskLogSummaryDto
    {
        public DateTime DueDate { get; set; }
        public string TaskName {  get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;// e.g., "30-minute walk completed"

    }
}

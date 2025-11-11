using System;

namespace Patient_Monitoring.DTOs.PatientDashboard
{
    public class WellnessProgressDto
    {
        public string PlanName { get; set; } = string.Empty; // e.g., "Exercise Plan"
        public string Goal { get; set; } = string.Empty; // e.g., "3 times per week"
        public int ProgressPercentage { get; set; } // e.g., 75
        public string MotivationQuote { get; set; } = string.Empty; // e.g., "Feeling more energetic..."
    }
}


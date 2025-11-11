using System;

namespace Patient_Monitoring.DTOs.PatientDashboard
{
    public class PrescriptionSummaryDto
    {
        public int PrescriptionId { get; set; }
        public string MedicationName { get; set; } = string.Empty;
        public string DosageAndInstructions { get; set; } = string.Empty;  // e.g., "10mg - Take once daily with food"
       
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;

    }
}

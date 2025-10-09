namespace Patient_Monitoring.DTOs
{
    public class PrescriptionDTO
    {
        public required string PrescriptionId { get; set; }
        public required string PatientId { get; set; }
        public required string MedicationName { get; set; }
        public required string Dosage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public required string PrescribingDoctorId { get; set; }
        public required string Instruction { get; set; }
        public required string Status { get; set; } // active, completed, discontinued
    }
}

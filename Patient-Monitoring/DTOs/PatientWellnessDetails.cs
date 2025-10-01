namespace Patient_Monitoring.DTOs
{
    public class PatientWellnessDetails
    {
        public string PatientID { get; set; } = string.Empty;
        public string PatientFirstName { get; set; } = string.Empty;
        public string PatientLastName { get; set; } = string.Empty;
        public string? DiseaseName { get; set; }
        public string Description { get; set; } = string.Empty; // Diagnosis description
        public string? SpecializedDoctorName { get; set; }
        public IEnumerable<PlanAssignment> AssignedPlans { get; set; } = Enumerable.Empty<PlanAssignment>();
    }
}

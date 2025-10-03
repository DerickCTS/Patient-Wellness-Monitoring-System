namespace Patient_Monitoring.DTOs
{
    public class PatientWellnessDTO
    {
        public string PatientID { get; set; } = string.Empty;
        
        public string FirstName { get; set; } = string.Empty;
        
        public string LastName { get; set; } = string.Empty;

        public List<DiagnosisDTO>? Diagnoses { get; set; }

        public List<PatientPlanDTO> AssignedPlans { get; set; } = new List<PatientPlanDTO>();
    }
}

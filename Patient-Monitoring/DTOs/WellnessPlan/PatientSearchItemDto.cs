using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.DTOs
{
    public class PatientSearchItemDto
    {
        public string PatientID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
    }
    public class DiagnosisDetailDto
    {
        public string DiseaseName { get; set; } = string.Empty;
        public string DiagnosisId { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public DateTime DiagnosisDate { get; set; }

    }
    public class MedicationDetailDto
    {
        public string MedicationName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
    public class WellnessPlanDetailDto
    {
        public string PlanName { get; set; } = string.Empty;
        public int FrequencyCount { get; set; }
        public string FrequencyUnit { get; set; } = string.Empty;
        public string WellnessType { get; set; } = null!;
    }

    public class PatientFullDetailDto
    {
        // Patient Model Details (Personal/Contact Info)
        public string PatientID { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string BloodGroup { get; set; } = string.Empty; // From Patient Model
        public string ContactNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string EmergencyContact { get; set; } = string.Empty;
        public string PersonalizedDoctor { get; set; } = string.Empty;

        // Diagnosis (from Diagnosis Model)
        public List<DiagnosisDetailDto> Diagnoses { get; set; } = new List<DiagnosisDetailDto>();

        // Prescriptions (from Prescriptions Model)
        public List<MedicationDetailDto> Medications { get; set; } = new List<MedicationDetailDto>();

        // Wellness Plans (from PatientPlan Model)
        // Note: This list will be empty if there are no plans, fulfilling the requirement.
        public List<WellnessPlanDetailDto> WellnessPlans { get; set; } = new List<WellnessPlanDetailDto>();

    }
    public class AssignPlanRequest
    {
        public string DoctorId { get; set; } = string.Empty;
        public string PatientId { get; set; } = string.Empty;
        public string PlanName { get; set; } = string.Empty;
        public string Goal { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Instruction { get; set; } = new List<string>();
        public List<string> Benefits { get; set; } = new List<string>();
        public List<string> Safety { get; set; } = new List<string>();
        public int FrequencyCount { get; set; }
        public string FrequencyUnit { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class DiagnosisDetailsDto
    {
        
        public string DiseaseDescription { get; set; } // Description from the Disease table
        public string DiagnosisDescription { get; set; } // Description from the Diagnoses table
       
    }


}

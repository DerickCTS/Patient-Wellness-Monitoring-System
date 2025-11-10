using System.Drawing;

namespace Patient_Monitoring.DTOs.WellnessPlan
{
    public class PatientSearchItemDto
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string ProfileImage { get; set; } = string.Empty;
        public string BloodGroup { get; set; } = string.Empty;
    }
    public class DiagnosisDetailDto
    {
        public string DiseaseName { get; set; } = string.Empty;
        public int DiagnosisId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public DateTime DiagnosisDate { get; set; }

    }
    public class MedicationDetailDto
    {
        public string MedicationName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive => DateTime.Now > EndDate;
    }
    
    public class WellnessPlanDetailDto
    {
        public int? PlanId { get; set; }
        public string PlanName { get; set; } = string.Empty;
        public string Goal { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string PlanImage { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string WellnessType { get; set; } = null!;
    }

    public class PatientFullDetailDto
    {
        // Patient Model Details (Personal/Contact Info)
        public int PatientId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string BloodGroup { get; set; } = string.Empty; // From Patient Model
        public string ContactNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string EmergencyContactNumber { get; set; } = string.Empty;
        public string EmergencyContactName { get; set; } = string.Empty;
        public string PersonalizedDoctor { get; set; } = string.Empty;

        // Diagnosis (from Diagnosis Model)
        public List<DiagnosisDetailDto> Diagnoses { get; set; } = new List<DiagnosisDetailDto>();


        // Wellness Plans (from PatientPlan Model)
        // Note: This list will be empty if there are no plans, fulfilling the requirement.
        public List<WellnessPlanDetailDto> WellnessPlans { get; set; } = new List<WellnessPlanDetailDto>();

    }
    public class DiagnosisDetailsDto
    {
        public string DiagnosedBy { get; set; }
        public string DiseaseDescription { get; set; } // Description from the Disease table
        public string DiagnosisDescription { get; set; } // Description from the Diagnoses table
        public List<MedicationDetailDto> MedicationDetails { get; set; }
    }


}

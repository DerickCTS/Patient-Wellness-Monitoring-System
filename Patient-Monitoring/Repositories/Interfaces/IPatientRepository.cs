using Patient_Monitoring.Models;

namespace Patient_Monitoring.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        //✅ Confirmed
        Task<Patient?> GetPatientByEmailAsync(string email);
        Task<bool> AddNewPatientAsync(Patient patient);


        Task<List<Patient>> SearchPatientsByIdOrNameAsync(string? patientName, int? patientId);
        Task<Patient?> GetPatientByIdAsync(int patientId);
        Task<bool> IsDoctorAssigned(int patientId, int doctorId);
        Task AddWellnessPlanAsync(WellnessPlan plan);
        Task AddPatientPlanAssignmentAsync(PatientPlanAssignment assignment);
        Task AddAssignmentPlanDetailsAsync(List<AssignmentPlanDetail> details);  
        Task<Diagnosis> GetDiagnosisWithDiseaseAsync(int diagnosisId);
    }
}


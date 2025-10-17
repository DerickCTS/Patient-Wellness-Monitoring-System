using Patient_Monitoring.Models;

namespace Patient_Monitoring.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        Task<List<Patient>> SearchPatientsByIdOrNameAsync(string? patientName, string? patientId);
        Task<Patient?> GetPatientByIdAsync(string patientId);
        Task<bool> IsDoctorAssigned(string patientId, string doctorId);
        Task AddWellnessPlanAsync(WellnessPlan plan);
        Task AddPatientPlanAssignmentAsync(PatientPlanAssignment assignment);
        Task AddAssignmentPlanDetailsAsync(List<AssignmentPlanDetail> details);
        Task SaveChangesAsync();
       
           
            Task<Diagnosis> GetDiagnosisWithDiseaseAsync(string diagnosisId);
        }
    }


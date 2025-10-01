using Patient_Monitoring.DTOs;
using Patient_Monitoring.Models;

namespace Patient_Monitoring.Repository.Interface
{
    /// <summary>
    /// Defines the methods for data access related to wellness plans and patient records.
    /// </summary>
    public interface IWellnessPlanRepository
    {
        // Retrieval methods (return entity models)
        Task<Patient_Detail?> GetPatientByIdAsync(string patientId);
        Task<Patient_Diagnosis?> GetPatientDiagnosisAsync(string patientId);
        Task<Disease?> GetDiseaseByIdAsync(string diseaseId);
        Task<Doctor_Detail?> GetSpecializedDoctorByPatientIdAsync(string patientId);
        Task<IEnumerable<Wellness_Plan>> GetAssignedPlansByPatientIdAsync(string patientId);
        Task<bool> PlanExistsAsync(string planId);
        Task<bool> IsPlanAssignedAsync(string patientId, string planId);

        // Write methods (accept entity models)
        void AddNewPlanAsync(Wellness_Plan newPlan);
        void AddPlanAssignmentAsync(Patient_Plan_Mapper patientPlanMapper);

        // Persistence
        Task SaveChangesAsync();
    }
}



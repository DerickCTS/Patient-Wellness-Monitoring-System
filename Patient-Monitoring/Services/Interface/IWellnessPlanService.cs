using Patient_Monitoring.DTOs;
using Patient_Monitoring.Models;


namespace Patient_Monitoring.Services.Interface
{
    /// <summary>
    /// Interface defining the business logic contract for managing patient wellness plans.
    /// </summary>
    public interface IWellnessPlanService
    {
        
        Task<PatientDetails?> GetPatientDetailsAsync(string patientId);

        Task<(PlanAssignment?, string?)> AssignPlanAsync(string patientId, Wellness_Plan newPlan);
    }
}



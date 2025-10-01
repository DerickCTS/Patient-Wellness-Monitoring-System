using Patient_Monitoring.DTOs;
using Patient_Monitoring.Models;


namespace Patient_Monitoring.Services.Interface
{
    public interface IWellnessPlanService
    {
        Task<PatientWellnessDetails?> GetPatientDetailsAsync(string patientId, string patientName);

        Task<(PlanAssignment?, string?)> AssignPlanAsync(string patientId, Wellness_Plan newPlan);
    }
}



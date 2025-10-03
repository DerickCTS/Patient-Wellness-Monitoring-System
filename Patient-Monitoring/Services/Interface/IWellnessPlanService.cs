using Patient_Monitoring.DTOs;
using Patient_Monitoring.Models;


namespace Patient_Monitoring.Services.Interface
{
    public interface IWellnessPlanService
    {
        Task<PatientWellnessDTO?> GetPatientDetailsAsync(string patientId, string patientName);

        //Task<(PatientPlanDTO?, string?)> AssignPlanAsync(string patientId, Wellness_Plan newPlan);
    }
}



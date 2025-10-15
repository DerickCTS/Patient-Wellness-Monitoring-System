using Patient_Monitoring.Models;

// Assuming models are here

namespace Patient_Monitoring.Repositories.Interfaces
{
    public interface IWellnessPlanRepository
    {
        // For 'Use Template'
        Task<IEnumerable<WellnessPlan>> GetAllTemplateCards();
        Task<WellnessPlan?> GetTemplatePlanAsync(string planId);
        Task<List<WellnessPlanDetail>> GetTemplatePlanDetailsAsync(string planId);
        // For Plan Assignment (both flows)
        //Task<PatientPlanAssignment> AddAssignmentAsync(PatientPlanAssignment assignment);
        Task AddAssignmentDetailsAsync(IEnumerable<AssignmentPlanDetail> details);

        Task AddWellnessPlanAsync(WellnessPlan plan);

        Task AddPatientPlanAssignment(PatientPlanAssignment assignment);

        Task AddAssignmentPlanDetails(List<AssignmentPlanDetail> details);
    }
}
using Patient_Monitoring.Models;
 // Assuming models are here

namespace Patient_Monitoring.Repository.Interfaces
{
    public interface IWellnessPlanRepository
    {
        // For 'Use Template'
        Task<IEnumerable<WellnessPlan>> GetAllTemplateCards();
        Task<WellnessPlan?> GetTemplatePlanAsync(string planId);
        Task<List<WellnessPlanDetail>> GetTemplatePlanDetailsAsync(string planId);
        // For Plan Assignment (both flows)
        Task<PatientPlanAssignment> AddAssignmentAsync(PatientPlanAssignment assignment);
        Task<IEnumerable<AssignmentPlanDetail>> AddAssignmentDetailsAsync(IEnumerable<AssignmentPlanDetail> details);
    }
}
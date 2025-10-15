using Patient_Monitoring.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
 // Assuming models are here

namespace Patient_Monitoring.Repository.Interface
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
using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repositories.Interfaces;



namespace Patient_Monitoring.Repositories.Implementations
{
    public class WellnessPlanRepository : IWellnessPlanRepository
    {
        private readonly PatientMonitoringDbContext _context; // Your DbContext

        public WellnessPlanRepository(PatientMonitoringDbContext context)
        {
            _context = context;
        }

        // --- 'Use Template' Flow ---

        public async Task AddWellnessPlanAsync(WellnessPlan plan)
        {
            await _context.WellnessPlans.AddAsync(plan);
            await _context.SaveChangesAsync();
        }

        public async Task AddPatientPlanAssignment(PatientPlanAssignment assignment)
        {
            await _context.PatientPlanAssignments.AddAsync(assignment);
            await _context.SaveChangesAsync();
        }

        public async Task AddAssignmentPlanDetails(List<AssignmentPlanDetail> details)
        {
            await _context.AssignmentPlanDetails.AddRangeAsync(details);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<WellnessPlan>> GetAllTemplateCards()
        {
            // Fetch template cards data: Plan Name, Goal, Image
            return await _context.WellnessPlans
                                 .Where(wp => wp.IsTemplate)
                                 .Select(p => p)
                                 .ToListAsync();
        }

        public async Task<WellnessPlan?> GetTemplatePlanAsync(int planId)
        {
            return await _context.WellnessPlans
                                 .FirstOrDefaultAsync(p => p.PlanId == planId);
        }

        // --- Assignment Flow (Both) ---

        public async Task<PatientPlanAssignment> AddAssignmentAsync(PatientPlanAssignment assignment)
        {
            _context.PatientPlanAssignments.Add(assignment);
            await _context.SaveChangesAsync();
            return assignment;
        }

        public async Task AddAssignmentDetailsAsync(IEnumerable<AssignmentPlanDetail> details)
        {
            _context.AssignmentPlanDetails.AddRange(details);
            await _context.SaveChangesAsync();
        }

        public async Task<List<WellnessPlanDetail>> GetTemplatePlanDetailsAsync(int planId)
        {
            return await _context.WellnessPlanDetails.Where(wpd => wpd.PlanId == planId).ToListAsync();
        }
    }
}

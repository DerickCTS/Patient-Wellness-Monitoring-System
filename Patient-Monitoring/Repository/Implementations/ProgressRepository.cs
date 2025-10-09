using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repository.Interfaces;

namespace Patient_Monitoring.Repository.Implementations
{
    public class ProgressRepository : IProgressRepository
    {
        private readonly PatientMonitoringDbContext _context;

        public ProgressRepository(PatientMonitoringDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DailyTaskLog>> GetWellnessTasksForPatientAsync(string patientId, DateTime startDate, DateTime endDate)
        {
            return await _context.DailyTaskLogs
                .Include(log => log.PatientPlan)
                    .ThenInclude(assignment => assignment.AssignedWellnessPlan)
                .Where(log => log.PatientPlan.PatientId == patientId && log.TaskDate >= startDate && log.TaskDate <= endDate)
                .ToListAsync();
        }

        public async Task<DailyTaskLog?> GetTaskLogByIdAsync(int logId)
        {
            return await _context.DailyTaskLogs.FindAsync(logId);
        }

        public async Task<PatientPlanAssignment?> GetAssignmentByIdAsync(int assignmentId)
        {
            // Assuming you have a Doctors table to join with
            return await _context.PatientPlanAssignments
                .Include(a => a.WellnessPlan)
                //.Include(a => a.AssignedByDoctor) // You would join to get the Doctor's name
                .FirstOrDefaultAsync(a => a.AssignmentId == assignmentId);
        }

        public async Task<IEnumerable<AssignmentPlanDetail>> GetCustomPlanDetailsAsync(int assignmentId)
        {
            return await _context.AssignmentPlanDetails
                .Where(d => d.AssignmentId == assignmentId)
                .OrderBy(d => d.DisplayOrder)
                .ToListAsync();
        }

        public async Task<IEnumerable<WellnessPlanDetail>> GetTemplatePlanDetailsAsync(int planId)
        {
            return await _context.WellnessPlanDetails
                .Where(d => d.PlanId == planId)
                .OrderBy(d => d.DisplayOrder)
                .ToListAsync();
        }

        public async Task<bool> UpdateTaskLogAsync(DailyTaskLog taskLog)
        {
            _context.DailyTaskLogs.Update(taskLog);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

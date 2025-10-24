using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.Models;

public class ProgressRepository : IProgressRepository
{
    private readonly PatientMonitoringDbContext _context;

    public ProgressRepository(PatientMonitoringDbContext context)
    {
        _context = context;
    }

    public async Task<List<PatientPlanAssignment>> GetActiveAssignmentsWithTasksAsync(int patientId)
    {
        return await _context.PatientPlanAssignments
            .Include(a => a.AssignedWellnessPlan)
            .Include(a => a.TaskLogs) 
            .Where(a => a.PatientId == patientId && a.IsActive)
            .ToListAsync();
    }

    public async Task<PatientPlanAssignment?> GetAssignmentDetailsAsync(int assignmentId)
    {
        return await _context.PatientPlanAssignments
            .Include(a => a.AssignedWellnessPlan)
                .ThenInclude(wp => wp.WellnessPlanDetails)
            .Include(a => a.AssigningDoctor)
            .Include(a => a.AssignmentPlanDetails)
            .FirstOrDefaultAsync(a => a.AssignmentId == assignmentId);
    }

    public async Task<TaskLog?> GetTaskLogByIdAsync(int taskLogId)
    {
        return await _context.TaskLogs.FirstOrDefaultAsync(t => t.LogId == taskLogId);
    }

    public async Task<List<TaskLog>> GetTaskLogsForPeriodAsync(int patientId, DateTime startDate, DateTime endDate)
    {
        var assignmentIds = await _context.PatientPlanAssignments
            .Where(a => a.PatientId == patientId)
            .Select(a => a.AssignmentId)
            .ToListAsync();

        return await _context.TaskLogs
            .Where(t => assignmentIds.Contains(t.AssignmentId) && t.DueDate >= startDate && t.DueDate <= endDate)
            .OrderBy(t => t.DueDate)
            .ToListAsync();
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
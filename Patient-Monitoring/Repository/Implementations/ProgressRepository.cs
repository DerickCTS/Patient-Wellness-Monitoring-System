using Patient_Monitoring.Data;
using Patient_Monitoring.Models;

public class ProgressRepository : IProgressRepository
{
    private readonly PatientMonitoringDbContext _context;

    public ProgressRepository(PatientMonitoringDbContext context)
    {
        _context = context;
    }

    public async Task<List<PatientPlanAssignment>> GetActiveAssignmentsWithTasksAsync(string patientId)
    {
        var today = DateTime.Today;
        // Logic to find the start of the current week (assuming Sunday is the start)
        var startOfWeek = today.AddDays(-(int)today.DayOfWeek);

        return await _context.PatientPlanAssignments
            .Include(a => a.AssignedWellnessPlan)
            .Include(a => a.DailyTaskLogs) // EF Core will use the correct name 'TaskLogs'
            .Where(a => a.PatientId == patientId && a.IsActive && a.StartDate <= today)
            .ToListAsync();
    }

    public async Task<PatientPlanAssignment?> GetAssignmentDetailsAsync(string assignmentId)
    {
        return await _context.PatientPlanAssignments
            .Include(a => a.AssignedWellnessPlan)
                .ThenInclude(wp => wp.WellnessPlanDetails)
            .Include(a => a.AssigningDoctor)
            .Include(a => a.AssignmentPlanDetails)
            .FirstOrDefaultAsync(a => a.AssignmentId == assignmentId);
    }

    public async Task<TaskLog?> GetTaskLogByIdAsync(string taskLogId)
    {
        return await _context.TaskLogs.FirstOrDefaultAsync(t => t.LogId == taskLogId);
    }

    public async Task<List<TaskLog>> GetTaskLogsForPeriodAsync(string patientId, DateTime startDate, DateTime endDate)
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
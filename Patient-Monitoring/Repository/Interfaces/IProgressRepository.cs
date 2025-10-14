using Patient_Monitoring.Models;

public interface IProgressRepository
{
    Task<List<PatientPlanAssignment>> GetActiveAssignmentsWithTasksAsync(string patientId);
    Task<PatientPlanAssignment?> GetAssignmentDetailsAsync(string assignmentId);
    Task<TaskLog?> GetTaskLogByIdAsync(string taskLogId);
    Task<List<TaskLog>> GetTaskLogsForPeriodAsync(string patientId, DateTime startDate, DateTime endDate);
    Task<bool> SaveChangesAsync();
}
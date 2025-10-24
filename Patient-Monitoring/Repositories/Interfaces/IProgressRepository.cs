using Patient_Monitoring.Models;

public interface IProgressRepository
{
    Task<List<PatientPlanAssignment>> GetActiveAssignmentsWithTasksAsync(int patientId);
    Task<PatientPlanAssignment?> GetAssignmentDetailsAsync(int assignmentId);
    Task<TaskLog?> GetTaskLogByIdAsync(int taskLogId);
    Task<List<TaskLog>> GetTaskLogsForPeriodAsync(int patientId, DateTime startDate, DateTime endDate);
    Task<bool> SaveChangesAsync();
}
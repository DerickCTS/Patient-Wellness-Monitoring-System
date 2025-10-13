using Patient_Monitoring.Models;

namespace Patient_Monitoring.Repository.Interfaces
{
    public interface IProgressRepository
    {
        Task<IEnumerable<TaskLog>> GetWellnessTasksForPatientAsync(string patientId, DateTime startDate, DateTime endDate);
        Task<TaskLog?> GetTaskLogByIdAsync(int logId);
        Task<PatientPlanAssignment?> GetAssignmentByIdAsync(int assignmentId);
        Task<IEnumerable<AssignmentPlanDetail>> GetCustomPlanDetailsAsync(int assignmentId);
        Task<IEnumerable<WellnessPlanDetail>> GetTemplatePlanDetailsAsync(int planId);
        Task<bool> UpdateTaskLogAsync(TaskLog taskLog);
    }
}

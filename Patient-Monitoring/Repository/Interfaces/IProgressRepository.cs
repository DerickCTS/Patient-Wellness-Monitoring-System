using Patient_Monitoring.Models;

namespace Patient_Monitoring.Repository.Interfaces
{
    public interface IProgressRepository
    {
        Task<IEnumerable<DailyTaskLog>> GetWellnessTasksForPatientAsync(string patientId, DateTime startDate, DateTime endDate);
        Task<DailyTaskLog?> GetTaskLogByIdAsync(int logId);
        Task<PatientPlanAssignment?> GetAssignmentByIdAsync(int assignmentId);
        Task<IEnumerable<AssignmentPlanDetail>> GetCustomPlanDetailsAsync(int assignmentId);
        Task<IEnumerable<WellnessPlanDetail>> GetTemplatePlanDetailsAsync(int planId);
        Task<bool> UpdateTaskLogAsync(DailyTaskLog taskLog);
    }
}

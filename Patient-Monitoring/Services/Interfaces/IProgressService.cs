using Patient_Monitoring.DTOs;

namespace Patient_Monitoring.Services.Interfaces
{
    public interface IProgressService
    {
        Task<IEnumerable<PlanCardDTO>> GetAssignedPlansAsync(string patientId, string timeframe);
        Task<PlanDetailDTO?> GetPlanDetailsAsync(int logId);
        Task<bool> UpdateTaskStatusAsync(int logId, string newStatus);
    }
}

public interface IProgressService
{
    Task<List<AssignedPlanCardDto>> GetAssignedPlanCardsAsync(string patientId, string statusFilter, string categoryFilter, string dateFilter);
    Task<PlanDetailDto?> GetPlanDetailsAsync(string assignmentId);
    Task<bool> UpdateTaskStatusAsync(string taskLogId, UpdateTaskStatusDto updateDto);
    Task<DashboardDto> GetDashboardDataAsync(string patientId);
}
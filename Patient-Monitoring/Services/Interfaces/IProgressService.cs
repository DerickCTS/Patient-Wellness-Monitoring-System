public interface IProgressService
{
    Task<List<AssignedPlanCardDto>> GetAssignedPlanCardsAsync(int patientId, string statusFilter, string categoryFilter, string dateFilter);
    Task<PlanDetailDto?> GetPlanDetailsAsync(int assignmentId);
    Task<bool> UpdateTaskStatusAsync(int taskLogId, UpdateTaskStatusDto updateDto);
    Task<DashboardDto> GetDashboardDataAsync(int patientId, int? year = null);

    Task<List<ActivityCalendarDay>> GetActivityCalendarAsync(int patientId, int year);
}
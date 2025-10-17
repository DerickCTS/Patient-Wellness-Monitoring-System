using Patient_Monitoring.DTOs.Notification;
using Patient_Monitoring.Models;

namespace Patient_Monitoring.Services.Interfaces
{
    /// <summary>
    /// Interface for notification business logic.
    /// </summary>
    public interface INotificationService
    {
        Task ScheduleNotificationAsync(NotificationDTO dto);
        Task SendNotificationAsync(NotificationDTO dto);
        Task<IEnumerable<Notification>> GetNotificationsAsync(string patientId);
        Task<Notification?> GetByIdAsync(int id);
        Task MarkAsReadAsync(int id);
        Task MarkAsTakenAsync(int id);
        Task MarkAsDoneAsync(int id);
        Task<bool> DeleteNotificationAsync(int id);


    }
}

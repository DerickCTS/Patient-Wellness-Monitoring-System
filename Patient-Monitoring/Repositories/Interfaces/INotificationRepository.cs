using Patient_Monitoring.Models;

namespace Patient_Monitoring.Repositories.Interfaces
{
    /// <summary>
    /// Interface for notification data access.
    /// </summary>
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetByPatientAsync(int patientId);
        Task<Notification?> GetByIdAsync(int id);
        Task<IEnumerable<Notification>> GetDueNotificationsAsync(DateTime now);
        Task AddAsync(Notification notification);
        Task UpdateAsync(Notification notification);
        Task SaveAsync();
        void Delete(Notification notification);

    }
}

using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repositories.Interface;

namespace Patient_Monitoring.Repositories.Implementation
{
    /// <summary>
    /// Concrete implementation of notification repository.
    /// </summary>
    public class NotificationRepository(PatientMonitoringDbContext context) : INotificationRepository
    {
        private readonly PatientMonitoringDbContext _context = context;

        public async Task<IEnumerable<Notification>> GetByPatientAsync(string patientId) =>
            await _context.Notifications
                .Where(n => n.PatientId == patientId)
                .OrderByDescending(n => n.ScheduledAt)
                .ToListAsync();

        public async Task<Notification?> GetByIdAsync(int id) =>
            await _context.Notifications.FindAsync(id);

        public async Task<IEnumerable<Notification>> GetDueNotificationsAsync(DateTime now) =>
            await _context.Notifications
                .Where(n => !n.IsRead  &&  n.ScheduledAt <= now)
                .ToListAsync();

        public async Task AddAsync(Notification notification) =>
            await _context.Notifications.AddAsync(notification);

        public async Task UpdateAsync(Notification notification) =>
            _context.Notifications.Update(notification);

        public async Task SaveAsync() => await _context.SaveChangesAsync();
        public void Delete(Notification notification)
        {
            _context.Notifications.Remove(notification);
        }

    }
}

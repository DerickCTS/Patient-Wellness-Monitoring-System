using NuGet.Protocol.Core.Types;
using Patient_Monitoring.DTOs.Notification;
using Patient_Monitoring.Enums;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repository.Interface;
using Patient_Monitoring.Services.Interface;

public class NotificationService(INotificationRepository repo) : INotificationService
{
    private readonly INotificationRepository _repo = repo;

    public async Task ScheduleNotificationAsync(NotificationDTO dto)
    {
        var adjustedTime = dto.Type switch
        {
            NotificationType.Medication => dto.ScheduledAt.AddMinutes(-15),
            NotificationType.Appointment => dto.ScheduledAt.AddDays(-1),
            NotificationType.WellnessActivity => dto.ScheduledAt.Date.AddHours(7),
            _ => dto.ScheduledAt
        };

        var notification = new Notification
        {
            Title = dto.Title,
            Message = dto.Message,
            ScheduledAt = adjustedTime,
            Type = dto.Type,
            PatientId = dto.PatientId,
            CreatedAt = DateTime.UtcNow,
            IsRead = false,
            
        };

        await _repo.AddAsync(notification);
        await _repo.SaveAsync();
    }

    public async Task SendNotificationAsync(NotificationDTO dto)
    {
        var notification = new Notification
        {
            Title = dto.Title,
            Message = dto.Message,
            ScheduledAt = DateTime.UtcNow,
            Type = dto.Type,
            PatientId = dto.PatientId,
            CreatedAt = DateTime.UtcNow,
            IsRead = false,
            
        };

        await _repo.AddAsync(notification);
        await _repo.SaveAsync();
    }

    public async Task<IEnumerable<Notification>> GetNotificationsAsync(string patientId) =>
        await _repo.GetByPatientAsync(patientId);

    public async Task<Notification?> GetByIdAsync(int id) =>
        await _repo.GetByIdAsync(id);

    public async Task MarkAsReadAsync(int id)
    {
        var n = await _repo.GetByIdAsync(id);
        if (n != null)
        {
            n.IsRead = true;
            await _repo.UpdateAsync(n);
            await _repo.SaveAsync();
        }
    }

    public async Task MarkAsTakenAsync(int id)
    {
        var n = await _repo.GetByIdAsync(id);
        if (n != null)
        {
            
            await _repo.UpdateAsync(n);
            await _repo.SaveAsync();
        }
    }

    public async Task MarkAsDoneAsync(int id)
    {
        var n = await _repo.GetByIdAsync(id);
        if (n != null)
        {
            
            await _repo.UpdateAsync(n);
            await _repo.SaveAsync();
        }
    }
    public async Task<bool> DeleteNotificationAsync(int id)
    {
        var notification = await _repo.GetByIdAsync(id);
        if (notification == null)
            return false;

        _repo.Delete(notification);
        await _repo.SaveAsync();
        return true;
    }


}

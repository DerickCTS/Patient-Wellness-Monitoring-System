using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.DTOs.Notification;
using Patient_Monitoring.Enums;
using Patient_Monitoring.Models;
using Patient_Monitoring.Services.Interfaces;

namespace Patient_Monitoring.Jobs
{
    /// <summary>
    /// Scheduled job that sends medicine, appointment, and wellness reminders.
    /// </summary>
    public class ScheduledNotificationJob
    {
        private readonly INotificationService _notificationService;
        private readonly PatientMonitoringDbContext _context;

        public ScheduledNotificationJob(INotificationService notificationService, PatientMonitoringDbContext context)
        {
            _notificationService = notificationService;
            _context = context;
        }


        /// <summary>
        /// Maps time slot labels to actual time ranges.
        /// </summary>
        private (TimeSpan start, TimeSpan end)? GetTimeRange(string slot)
        {
            return slot.ToLower() switch
            {
                "forenoon" => (new TimeSpan(8, 0, 0), new TimeSpan(11, 59, 59)),
                "noon" => (new TimeSpan(12, 0, 0), new TimeSpan(15, 0, 0)),
                "evening" => (new TimeSpan(18, 0, 0), new TimeSpan(21, 0, 0)),
                _ => null
            };
        }

        /// <summary>
        /// Sends medicine reminders based on current time slot.
        /// </summary>
        public async Task SendMedicineRemindersAsync()
        {
            var now = DateTime.Now.TimeOfDay;

            var meds = await _context.Set<MedicationSchedule>()
                .Include(m => m.Prescription)
                .ToListAsync();

            foreach (var m in meds)
            {
                var range = GetTimeRange(m.TimeOfDay);
                if (range == null) continue;

                if (now >= range.Value.start && now <= range.Value.end)
                {
                    var dto = new NotificationDTO
                    {
                        PatientId = m.Prescription.PatientId,
                        Title = "Medicine Reminder",
                        Message = $"Take {m.Quantity} units of {m.Prescription.MedicationName} ({m.TimeOfDay})",
                        Type = NotificationType.MedicineReminder,
                        ScheduledAt = DateTime.Now
                    };

                    await _notificationService.SendNotificationAsync(dto);
                }
            }
        }

        /// <summary>
        /// Sends appointment reminders for tomorrow.
        /// </summary>
        public async Task SendAppointmentRemindersAsync()
        {
            var tomorrow = DateTime.Today.AddDays(1);

            var appointments = await _context.Set<Appointment>()
                .Where(a => a.AppointmentDate.Date == tomorrow)
                .ToListAsync();

            foreach (var a in appointments)
            {
                var dto = new NotificationDTO
                {
                    PatientId = a.PatientId,
                    Title = "Appointment Reminder",
                    Message = $"You have an appointment scheduled tomorrow. Reason: {a.Reason}",
                    Type = NotificationType.AppointmentReminder,
                    ScheduledAt = DateTime.Now
                };

                await _notificationService.SendNotificationAsync(dto);
            }
        }

        /// <summary>
        /// Sends wellness activity reminders based on today's plan.
        /// </summary>
        public async Task SendDailyWellnessRemindersAsync()
        {
            var today = DateTime.Today.DayOfWeek.ToString();

            var assignments = await _context.Set<PatientPlanAssignment>()
                .Include(a => a.AssignmentPlanDetails)
                .Include(a => a.AssignedWellnessPlan)
                .Where(a => a.IsActive && a.StartDate <= DateTime.Today &&
                            (a.EndDate == default || a.EndDate >= DateTime.Today))
                .ToListAsync();

            foreach (var assignment in assignments)
            {
                var todayDetails = assignment.AssignmentPlanDetails?
                    .Where(d => d.Content.Contains(today, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (todayDetails == null || !todayDetails.Any())
                    continue;

                foreach (var detail in todayDetails)
                {
                    var dto = new NotificationDTO
                    {
                        PatientId = assignment.PatientId,
                        Title = "Today's Wellness Activity",
                        Message = $"Activity: {detail.Content} (Plan: {assignment.AssignedWellnessPlan.PlanName})",
                        Type = NotificationType.DailyScheduleSummary,
                        ScheduledAt = DateTime.Now
                    };

                    await _notificationService.SendNotificationAsync(dto);
                }
            }
        }
    }
}

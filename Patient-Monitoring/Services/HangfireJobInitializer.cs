using Hangfire;
using Patient_Monitoring.Jobs;

namespace Patient_Monitoring.Services
{
    /// <summary>
    /// Initializes recurring Hangfire jobs for sending notifications.
    /// </summary>
    public class HangfireJobInitializer : IHostedService
    {
        private readonly IRecurringJobManager _jobManager;

        public HangfireJobInitializer(IRecurringJobManager jobManager)
        {
            _jobManager = jobManager;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("🚀 Initializing Hangfire notification jobs...");

            // ✅ Medicine reminders - Check every 5 minutes
            _jobManager.AddOrUpdate<ScheduledNotificationJob>(
                "medicine-reminders",
                job => job.SendMedicineRemindersAsync(),
                "*/5 * * * *"); // Every 5 minutes
            Console.WriteLine("✅ Medicine reminders job scheduled (every 5 minutes)");

            // ✅ Appointment reminders - Check once daily at 9 AM
            _jobManager.AddOrUpdate<ScheduledNotificationJob>(
                "appointment-reminders",
                job => job.SendAppointmentRemindersAsync(),
                Cron.Daily(9)); // Every day at 9 AM
            Console.WriteLine("✅ Appointment reminders job scheduled (daily at 9 AM)");

            // ✅ Daily wellness reminders - Check every 15 minutes during morning hours
            _jobManager.AddOrUpdate<ScheduledNotificationJob>(
                "wellness-reminders",
                job => job.SendDailyWellnessRemindersAsync(),
                "*/15 7-8 * * *"); // Every 15 minutes between 7–8 AM
            Console.WriteLine("✅ Wellness reminders job scheduled (7–8 AM, every 15 min)");

            Console.WriteLine("🎉 All Hangfire jobs initialized successfully!");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("🛑 Stopping Hangfire notification jobs...");
            return Task.CompletedTask;
        }
    }
}

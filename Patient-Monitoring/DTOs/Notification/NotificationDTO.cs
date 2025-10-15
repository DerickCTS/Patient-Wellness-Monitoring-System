using System.ComponentModel.DataAnnotations;
using Patient_Monitoring.Enums;

namespace Patient_Monitoring.DTOs.Notification
{
    /// <summary>
    /// DTO used to create a new notification.
    /// </summary>
    public class NotificationDTO
    {
        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Message { get; set; }

        [Required]
        public DateTime ScheduledAt { get; set; }

        [Required]
        public NotificationType Type { get; set; }

        [Required]
        public required string PatientId { get; set; }
    }
}

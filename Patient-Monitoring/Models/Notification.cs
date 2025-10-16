
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Patient_Monitoring.Enums;

namespace Patient_Monitoring.Models
{


    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        [Required]
        [StringLength(100)]
        public required string Title { get; set; }

        [Required]
        [StringLength(500)]
        public required string Message { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime ScheduledAt { get; set; }

        [Required]
        public bool IsRead { get; set; } = false;



        [Required]
        public NotificationType Type { get; set; }
        public Patient Patient { get; set; } = null!;
        [ForeignKey("Patient")]
        public string PatientId { get; set; } = null!;




    }
}